using ConsoleApp1.Commands;
using VehicleCore;

namespace ConsoleApp1;

class Program
{
    private static List<Vehicle> vehicles = new();
    private static XmlVehicleSerializer? serializer;
    private static MenuCommandRegistry menu = new();

    static void Main()
    {
        string baseDir = AppContext.BaseDirectory;
        PluginLoader.LoadFromFolder(Path.Combine(baseDir, "Plugins"));
        PluginLoader.LoadFriendPlugins(Path.Combine(baseDir, "Plugins", "FriendPlugins"));

        serializer = new XmlVehicleSerializer(PluginLoader.VehiclePlugins.Select(p => p.VehicleType));
        RegisterCommands();

        bool running = true;
        while (running)
        {
            Console.WriteLine("\nУПРАВЛЕНИЕ ТРАНСПОРТОМ (лаб. 6 — паттерны)");
            foreach (var cmd in menu.All)
                Console.WriteLine($"{cmd.Key} - {cmd.Title}");
            Console.Write("Выберите действие: ");
            string? choice = Console.ReadLine();
            if (!menu.TryExecute(choice))
                Console.WriteLine("Неверная команда.");
            if (choice == "0") running = false;
        }
    }

    static void RegisterCommands()
    {
        menu.Register(new DelegateCommand("1", "Добавить транспорт", AddVehicle));
        menu.Register(new DelegateCommand("2", "Показать список", ShowVehicles));
        menu.Register(new DelegateCommand("3", "Редактировать", EditVehicle));
        menu.Register(new DelegateCommand("4", "Удалить", RemoveVehicle));
        menu.Register(new DelegateCommand("5", "Сохранить", Save));
        menu.Register(new DelegateCommand("6", "Загрузить", Load));
        menu.Register(new DelegateCommand("7", "Загрузить плагин (.dll)", LoadPlugin));
        menu.Register(new DelegateCommand("8", "Загрузить плагин товарища (FriendPlugins)", LoadFriendPlugin));
        menu.Register(new DelegateCommand("9", "Настройки обработки", Settings));
        menu.Register(new DelegateCommand("0", "Выход", () => Console.WriteLine("Завершено.")));
    }

    static DataProcessingPipeline Pipeline() =>
        new(PluginLoader.ProcessingPlugins.Where(p => AppSettings.Instance.IsEnabled(p.Name)));

    static void AddVehicle()
    {
        var f = new Dictionary<string, Func<Vehicle>>
        {
            { "1", () => new Car() }, { "2", () => new Truck() },
            { "3", () => new Motorcycle() }, { "4", () => new Bus() },
            { "5", () => new ElectricCar() }, { "6", () => new SportCar() }
        };
        for (int i = 0; i < PluginLoader.VehiclePlugins.Count; i++)
            Console.WriteLine($"{7 + i} - {PluginLoader.VehiclePlugins[i].VehicleTypeName}");
        string? c = Console.ReadLine();
        if (c != null && f.TryGetValue(c, out var factory)) { var v = factory(); v.Edit(); vehicles.Add(v); }
        else if (int.TryParse(c, out int n) && n >= 7 && n < 7 + PluginLoader.VehiclePlugins.Count)
        { var v = PluginLoader.VehiclePlugins[n - 7].CreateVehicle(); v.Edit(); vehicles.Add(v); }
    }

    static void ShowVehicles()
    {
        if (vehicles.Count == 0) Console.WriteLine("Пусто.");
        for (int i = 0; i < vehicles.Count; i++) { Console.Write($"{i}: "); vehicles[i].DisplayInfo(); }
    }

    static void EditVehicle()
    {
        ShowVehicles();
        int i = int.Parse(Console.ReadLine() ?? "-1");
        if (i >= 0 && i < vehicles.Count) vehicles[i].Edit();
    }

    static void RemoveVehicle()
    {
        ShowVehicles();
        int i = int.Parse(Console.ReadLine() ?? "-1");
        if (i >= 0 && i < vehicles.Count) vehicles.RemoveAt(i);
    }

    static void Save()
    {
        byte[] data = Pipeline().ProcessBeforeSave(serializer!.SerializeToBytes(vehicles));
        File.WriteAllBytes("vehicles.dat", data);
        Console.WriteLine("Сохранено.");
    }

    static void Load()
    {
        if (!File.Exists("vehicles.dat")) return;
        byte[] raw = File.ReadAllBytes("vehicles.dat");
        vehicles = serializer!.DeserializeFromBytes(Pipeline().ProcessAfterLoad(raw));
        Console.WriteLine("Загружено.");
    }

    static void LoadPlugin()
    {
        Console.Write("Путь к DLL: ");
        string? p = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(p)) PluginLoader.LoadFromFile(p!.Trim());
    }

    static void LoadFriendPlugin()
    {
        Console.Write("Путь к DLL товарища: ");
        string? p = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(p)) PluginLoader.LoadFromFile(p!.Trim(), asFriendPlugin: true);
    }

    static void Settings()
    {
        if (PluginLoader.ProcessingPlugins.Count == 0)
        {
            Console.WriteLine("Нет плагинов обработки.");
            return;
        }

        Console.WriteLine("\n--- Настройки обработки (по номеру) ---");
        for (int i = 0; i < PluginLoader.ProcessingPlugins.Count; i++)
        {
            var p = PluginLoader.ProcessingPlugins[i];
            string s = AppSettings.Instance.IsEnabled(p.Name) ? "ВКЛ" : "ВЫКЛ";
            Console.WriteLine($"{i + 1}. [{s}] {p.Name}: {p.Description}");
        }

        Console.Write("Переключить номер (Enter — назад): ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            return;

        if (!int.TryParse(input, out int n) || n < 1 || n > PluginLoader.ProcessingPlugins.Count)
        {
            Console.WriteLine("Неверный номер.");
            return;
        }

        var plugin = PluginLoader.ProcessingPlugins[n - 1];
        bool newState = !AppSettings.Instance.IsEnabled(plugin.Name);
        AppSettings.Instance.SetEnabled(plugin.Name, newState);
        Console.WriteLine($"{plugin.Name}: {(newState ? "включён" : "выключен")}");
    }
}

internal sealed class DelegateCommand : IMenuCommand
{
    private readonly Action _action;
    public string Key { get; }
    public string Title { get; }
    public DelegateCommand(string key, string title, Action action)
    {
        Key = key; Title = title; _action = action;
    }
    public void Execute() => _action();
}
