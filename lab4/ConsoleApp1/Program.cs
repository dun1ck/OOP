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
        { "0", ExitProgram }
    };

    static void Main(string[] args)
    {
        string pluginsPath = Path.Combine(AppContext.BaseDirectory, "Plugins");
        PluginLoader.LoadFromFolder(pluginsPath);

        if (args.Length > 0 && File.Exists(args[0]))
            PluginLoader.LoadFromFile(args[0]);

        serializer = new XmlVehicleSerializer(
            PluginLoader.VehiclePlugins.Select(p => p.VehicleType));

        bool running = true;
        while (running)
        {
            ShowMenu();
            Console.Write("Выберите действие: ");
            string? choice = Console.ReadLine();

            if (choice != null && commands.TryGetValue(choice, out Action? action))
            {
                action();
                if (choice == "0")
                    running = false;
            }
            else
                Console.WriteLine("Неверная команда.");

            Console.WriteLine();
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("УПРАВЛЕНИЕ ТРАНСПОРТОМ (лаб. 4 — плагины иерархии)");
        Console.WriteLine("1 - Добавить транспорт");
        Console.WriteLine("2 - Показать список");
        Console.WriteLine("3 - Редактировать транспорт");
        Console.WriteLine("4 - Удалить транспорт");
        Console.WriteLine("5 - Сохранить в XML");
        Console.WriteLine("6 - Загрузить из XML");
        Console.WriteLine("7 - Загрузить плагин из файла (.dll)");
        Console.WriteLine("0 - Выход");
    }

    static void AddVehicle()
    {
        var factories = new Dictionary<string, Func<Vehicle>>
        {
            { "1", () => new Car() },
            { "2", () => new Truck() },
            { "3", () => new Motorcycle() },
            { "4", () => new Bus() },
            { "5", () => new ElectricCar() },
            { "6", () => new SportCar() }
        };

        Console.WriteLine("Выберите тип транспорта:");
        Console.WriteLine("1 - Легковой автомобиль");
        Console.WriteLine("2 - Грузовик");
        Console.WriteLine("3 - Мотоцикл");
        Console.WriteLine("4 - Автобус");
        Console.WriteLine("5 - Электромобиль");
        Console.WriteLine("6 - Спорткар");

        for (int i = 0; i < PluginLoader.VehiclePlugins.Count; i++)
            Console.WriteLine($"{7 + i} - {PluginLoader.VehiclePlugins[i].VehicleTypeName}");

        string? choice = Console.ReadLine();

        if (choice != null && factories.TryGetValue(choice, out Func<Vehicle>? factory))
        {
            Vehicle vehicle = factory();
            vehicle.Edit();
            vehicles.Add(vehicle);
            Console.WriteLine("Транспорт добавлен.");
        }
        else if (int.TryParse(choice, out int pluginIndex) &&
                 pluginIndex >= 7 &&
                 pluginIndex < 7 + PluginLoader.VehiclePlugins.Count)
        {
            Vehicle vehicle = PluginLoader.VehiclePlugins[pluginIndex - 7].CreateVehicle();
            vehicle.Edit();
            vehicles.Add(vehicle);
            Console.WriteLine("Транспорт добавлен.");
        }
        else
            Console.WriteLine("Неверный тип.");
    }

    static void ShowVehicles()
    {
        if (vehicles.Count == 0)
        {
            Console.WriteLine("Список пуст.");
            return;
        }

        for (int i = 0; i < vehicles.Count; i++)
        {
            Console.Write($"{i}: ");
            vehicles[i].DisplayInfo();
        }
    }

    static void EditVehicle()
    {
        ShowVehicles();
        Console.Write("Введите индекс: ");
        int index = int.Parse(Console.ReadLine() ?? "-1");

        if (index >= 0 && index < vehicles.Count)
        {
            vehicles[index].Edit();
            Console.WriteLine("Транспорт обновлен.");
        }
        else
            Console.WriteLine("Неверный индекс.");
    }

    static void RemoveVehicle()
    {
        ShowVehicles();
        Console.Write("Введите индекс: ");
        int index = int.Parse(Console.ReadLine() ?? "-1");

        if (index >= 0 && index < vehicles.Count)
        {
            vehicles.RemoveAt(index);
            Console.WriteLine("Транспорт удален.");
        }
        else
            Console.WriteLine("Неверный индекс.");
    }

    static void SaveToXml()
    {
        serializer!.Serialize(vehicles, "vehicles.xml");
        Console.WriteLine("Данные сохранены в vehicles.xml");
    }

    static void LoadFromXml()
    {
        if (File.Exists("vehicles.xml"))
        {
            vehicles = serializer!.Deserialize("vehicles.xml");
            Console.WriteLine("Данные загружены из vehicles.xml");
        }
        else
            Console.WriteLine("Файл не существует.");
    }

    static void LoadPluginFromFile()
    {
        Console.Write("Путь к DLL плагина: ");
        string? path = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(path))
            return;

        PluginLoader.LoadFromFile(path.Trim());
        serializer = new XmlVehicleSerializer(
            PluginLoader.VehiclePlugins.Select(p => p.VehicleType));
    }

    static void ExitProgram() => Console.WriteLine("Программа завершена.");
}
