using VehicleCore;

namespace ConsoleApp1;

class Program
{
    private static List<Vehicle> vehicles = new();
    private static XmlVehicleSerializer? serializer;

    private static Dictionary<string, Action> commands = new()
    {
        { "1", AddVehicle },
        { "2", ShowVehicles },
        { "3", EditVehicle },
        { "4", RemoveVehicle },
        { "5", SaveToXml },
        { "6", LoadFromXml },
        { "7", LoadPluginFromFile },
        { "8", ShowSettingsMenu },
        { "0", ExitProgram }
    };

    static void Main()
    {
        string pluginsPath = Path.Combine(AppContext.BaseDirectory, "Plugins");
        PluginLoader.LoadFromFolder(pluginsPath);
        serializer = new XmlVehicleSerializer(PluginLoader.VehiclePlugins.Select(p => p.VehicleType));

        bool running = true;
        while (running)
        {
            ShowMenu();
            Console.Write("Выберите действие: ");
            string? choice = Console.ReadLine();
            if (choice != null && commands.TryGetValue(choice, out Action? action))
            {
                action();
                if (choice == "0") running = false;
            }
            else Console.WriteLine("Неверная команда.");
            Console.WriteLine();
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("УПРАВЛЕНИЕ ТРАНСПОРТОМ (лаб. 5 — плагины обработки)");
        Console.WriteLine("1 - Добавить транспорт");
        Console.WriteLine("2 - Показать список");
        Console.WriteLine("3 - Редактировать");
        Console.WriteLine("4 - Удалить");
        Console.WriteLine("5 - Сохранить в файл");
        Console.WriteLine("6 - Загрузить из файла");
        Console.WriteLine("7 - Загрузить плагин (.dll)");
        Console.WriteLine("8 - Настройки обработки");
        Console.WriteLine("0 - Выход");
    }

    static DataProcessingPipeline CreatePipeline()
    {
        var enabled = PluginLoader.ProcessingPlugins
            .Where(p => AppSettings.Instance.IsEnabled(p.Name));
        return new DataProcessingPipeline(enabled);
    }

    static void AddVehicle()
    {
        var factories = new Dictionary<string, Func<Vehicle>>
        {
            { "1", () => new Car() }, { "2", () => new Truck() },
            { "3", () => new Motorcycle() }, { "4", () => new Bus() },
            { "5", () => new ElectricCar() }, { "6", () => new SportCar() }
        };
        Console.WriteLine("1-Легковой 2-Грузовик 3-Мотоцикл 4-Автобус 5-Электро 6-Спорткар");
        for (int i = 0; i < PluginLoader.VehiclePlugins.Count; i++)
            Console.WriteLine($"{7 + i} - {PluginLoader.VehiclePlugins[i].VehicleTypeName}");

        string? choice = Console.ReadLine();
        if (choice != null && factories.TryGetValue(choice, out var f))
        {
            var v = f(); v.Edit(); vehicles.Add(v);
            Console.WriteLine("Добавлено.");
        }
        else if (int.TryParse(choice, out int idx) && idx >= 7 && idx < 7 + PluginLoader.VehiclePlugins.Count)
        {
            var v = PluginLoader.VehiclePlugins[idx - 7].CreateVehicle();
            v.Edit(); vehicles.Add(v);
            Console.WriteLine("Добавлено.");
        }
        else Console.WriteLine("Неверный тип.");
    }

    static void ShowVehicles()
    {
        if (vehicles.Count == 0) { Console.WriteLine("Список пуст."); return; }
        for (int i = 0; i < vehicles.Count; i++) { Console.Write($"{i}: "); vehicles[i].DisplayInfo(); }
    }

    static void EditVehicle()
    {
        ShowVehicles();
        int index = int.Parse(Console.ReadLine() ?? "-1");
        if (index >= 0 && index < vehicles.Count) { vehicles[index].Edit(); Console.WriteLine("Обновлено."); }
        else Console.WriteLine("Неверный индекс.");
    }

    static void RemoveVehicle()
    {
        ShowVehicles();
        int index = int.Parse(Console.ReadLine() ?? "-1");
        if (index >= 0 && index < vehicles.Count) { vehicles.RemoveAt(index); Console.WriteLine("Удалено."); }
        else Console.WriteLine("Неверный индекс.");
    }

    static void SaveToXml()
    {
        byte[] xml = serializer!.SerializeToBytes(vehicles);
        byte[] processed = CreatePipeline().ProcessBeforeSave(xml);
        File.WriteAllBytes("vehicles.dat", processed);
        Console.WriteLine("Сохранено в vehicles.dat");
    }

    static void LoadFromXml()
    {
        if (!File.Exists("vehicles.dat")) { Console.WriteLine("Файл не найден."); return; }
        byte[] raw = File.ReadAllBytes("vehicles.dat");
        byte[] xml = CreatePipeline().ProcessAfterLoad(raw);
        vehicles = serializer!.DeserializeFromBytes(xml);
        Console.WriteLine("Загружено из vehicles.dat");
    }

    static void LoadPluginFromFile()
    {
        Console.Write("Путь к DLL: ");
        string? path = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(path)) return;
        PluginLoader.LoadFromFile(path.Trim());
        serializer = new XmlVehicleSerializer(PluginLoader.VehiclePlugins.Select(p => p.VehicleType));
    }

    static void ShowSettingsMenu()
    {
        if (PluginLoader.ProcessingPlugins.Count == 0)
        {
            Console.WriteLine("Нет загруженных плагинов обработки. Положите DLL в папку Plugins.");
            return;
        }

        bool inSettings = true;
        while (inSettings)
        {
            Console.WriteLine("\n--- Настройки обработки ---");
            int i = 1;
            foreach (var plugin in PluginLoader.ProcessingPlugins)
            {
                string state = AppSettings.Instance.IsEnabled(plugin.Name) ? "ВКЛ" : "ВЫКЛ";
                Console.WriteLine($"{i}. [{state}] {plugin.Name} — {plugin.Description}");
                i++;
            }
            Console.WriteLine("0 - Назад");
            Console.Write("Включить/выключить номер плагина (или 0): ");
            string? input = Console.ReadLine();
            if (input == "0") { inSettings = false; continue; }
            if (!int.TryParse(input, out int num) || num < 1 || num > PluginLoader.ProcessingPlugins.Count)
            {
                Console.WriteLine("Неверный номер.");
                continue;
            }
            var selected = PluginLoader.ProcessingPlugins[num - 1];
            bool newState = !AppSettings.Instance.IsEnabled(selected.Name);
            AppSettings.Instance.SetEnabled(selected.Name, newState);
            Console.WriteLine($"{selected.Name}: {(newState ? "включён" : "выключен")}");
        }
    }

    static void ExitProgram() => Console.WriteLine("Завершено.");
}
