using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

/*
 * Base abstract class for all vehicles.
 */
[XmlInclude(typeof(Car))]
[XmlInclude(typeof(Truck))]
[XmlInclude(typeof(Motorcycle))]
[XmlInclude(typeof(Bus))]
[XmlInclude(typeof(ElectricCar))]
[XmlInclude(typeof(SportCar))]
public abstract class Vehicle
{
    public string Brand { get; set; }

    public int Year { get; set; }

    /*
     * Displays object information.
     */
    public abstract void DisplayInfo();

    /*
     * Allows editing object properties.
     */
    public abstract void Edit();
}

/*
 * Car class.
 */
public class Car : Vehicle
{
    public int Doors { get; set; }

    public override void DisplayInfo()
    {
        Console.WriteLine(
            $"Легковой автомобиль | Марка: {Brand} | Год: {Year} | Дверей: {Doors}");
    }

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine();

        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine());

        Console.Write("Введите количество дверей: ");
        Doors = int.Parse(Console.ReadLine());
    }
}

/*
 * Truck class.
 */
public class Truck : Vehicle
{
    public double LoadCapacity { get; set; }

    public override void DisplayInfo()
    {
        Console.WriteLine(
            $"Грузовик | Марка: {Brand} | Год: {Year} | Грузоподъемность: {LoadCapacity}");
    }

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine();

        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine());

        Console.Write("Введите грузоподъемность: ");
        LoadCapacity = double.Parse(Console.ReadLine());
    }
}

/*
 * Motorcycle class.
 */
public class Motorcycle : Vehicle
{
    public bool HasSidecar { get; set; }

    public override void DisplayInfo()
    {
        Console.WriteLine(
            $"Мотоцикл | Марка: {Brand} | Год: {Year} | Люлька: {HasSidecar}");
    }

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine();

        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine());

        Console.Write("Есть ли люлька (true/false): ");
        HasSidecar = bool.Parse(Console.ReadLine());
    }
}

/*
 * Bus class.
 */
public class Bus : Vehicle
{
    public int PassengerCapacity { get; set; }

    public override void DisplayInfo()
    {
        Console.WriteLine(
            $"Автобус | Марка: {Brand} | Год: {Year} | Вместимость: {PassengerCapacity}");
    }

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine();

        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine());

        Console.Write("Введите пассажировместимость: ");
        PassengerCapacity = int.Parse(Console.ReadLine());
    }
}

/*
 * Electric car class.
 */
public class ElectricCar : Vehicle
{
    public int BatteryCapacity { get; set; }

    public override void DisplayInfo()
    {
        Console.WriteLine(
            $"Электромобиль | Марка: {Brand} | Год: {Year} | Батарея: {BatteryCapacity}");
    }

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine();

        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine());

        Console.Write("Введите емкость батареи: ");
        BatteryCapacity = int.Parse(Console.ReadLine());
    }
}

/*
 * Sport car class.
 */
public class SportCar : Vehicle
{
    public int MaxSpeed { get; set; }

    public override void DisplayInfo()
    {
        Console.WriteLine(
            $"Спорткар | Марка: {Brand} | Год: {Year} | Максимальная скорость: {MaxSpeed}");
    }

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine();

        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine());

        Console.Write("Введите максимальную скорость: ");
        MaxSpeed = int.Parse(Console.ReadLine());
    }
}

/*
 * XML serializer class.
 */
public class XmlVehicleSerializer
{
    /*
     * Saves vehicle list to XML file.
     */
    public void Serialize(List<Vehicle> vehicles, string path)
    {
        XmlSerializer serializer =
            new XmlSerializer(typeof(List<Vehicle>));

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(fs, vehicles);
        }
    }

    /*
     * Loads vehicle list from XML file.
     */
    public List<Vehicle> Deserialize(string path)
    {
        XmlSerializer serializer =
            new XmlSerializer(typeof(List<Vehicle>));

        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            return (List<Vehicle>)serializer.Deserialize(fs);
        }
    }
}

/*
 * Main application class.
 */
class Program
{
    /*
     * Vehicle collection.
     */
    private static List<Vehicle> vehicles =
        new List<Vehicle>();

    /*
     * Serializer object.
     */
    private static XmlVehicleSerializer serializer =
        new XmlVehicleSerializer();

    /*
     * Dictionary for menu commands.
     */
    private static Dictionary<string, Action> commands =
        new Dictionary<string, Action>
    {
        { "1", AddVehicle },
        { "2", ShowVehicles },
        { "3", EditVehicle },
        { "4", RemoveVehicle },
        { "5", SaveToXml },
        { "6", LoadFromXml },
        { "0", ExitProgram }
    };

    static void Main(string[] args)
    {
        bool running = true;

        while (running)
        {
            ShowMenu();

            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();

            /*
             * Executes selected command.
             */
            if (commands.ContainsKey(choice))
            {
                commands[choice].Invoke();

                if (choice == "0")
                {
                    running = false;
                }
            }
            else
            {
                Console.WriteLine("Неверная команда.");
            }

            Console.WriteLine();
        }
    }

    /*
     * Displays menu.
     */
    static void ShowMenu()
    {
        Console.WriteLine("УПРАВЛЕНИЕ ТРАНСПОРТОМ");
        Console.WriteLine("1 - Добавить транспорт");
        Console.WriteLine("2 - Показать список");
        Console.WriteLine("3 - Редактировать транспорт");
        Console.WriteLine("4 - Удалить транспорт");
        Console.WriteLine("5 - Сохранить в XML");
        Console.WriteLine("6 - Загрузить из XML");
        Console.WriteLine("0 - Выход");
    }

    /*
     * Adds vehicle to collection.
     */
    static void AddVehicle()
    {
        Dictionary<string, Func<Vehicle>> factories =
            new Dictionary<string, Func<Vehicle>>
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

        string choice = Console.ReadLine();

        if (factories.ContainsKey(choice))
        {
            Vehicle vehicle = factories[choice].Invoke();

            vehicle.Edit();

            vehicles.Add(vehicle);

            Console.WriteLine("Транспорт добавлен.");
        }
        else
        {
            Console.WriteLine("Неверный тип.");
        }
    }

    /*
     * Displays all vehicles.
     */
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

    /*
     * Edits vehicle.
     */
    static void EditVehicle()
    {
        ShowVehicles();

        Console.Write("Введите индекс: ");
        int index = int.Parse(Console.ReadLine());

        if (index >= 0 && index < vehicles.Count)
        {
            vehicles[index].Edit();

            Console.WriteLine("Транспорт обновлен.");
        }
        else
        {
            Console.WriteLine("Неверный индекс.");
        }
    }

    /*
     * Removes vehicle.
     */
    static void RemoveVehicle()
    {
        ShowVehicles();

        Console.Write("Введите индекс: ");
        int index = int.Parse(Console.ReadLine());

        if (index >= 0 && index < vehicles.Count)
        {
            vehicles.RemoveAt(index);

            Console.WriteLine("Транспорт удален.");
        }
        else
        {
            Console.WriteLine("Неверный индекс.");
        }
    }

    /*
     * Saves list to XML file.
     */
    static void SaveToXml()
    {
        serializer.Serialize(vehicles, "vehicles.xml");

        Console.WriteLine("Данные сохранены в vehicles.xml");
    }

    /*
     * Loads list from XML file.
     */
    static void LoadFromXml()
    {
        if (File.Exists("vehicles.xml"))
        {
            vehicles = serializer.Deserialize("vehicles.xml");

            Console.WriteLine("Данные загружены из vehicles.xml");
        }
        else
        {
            Console.WriteLine("Файл не существует.");
        }
    }

    /*
     * Exits program.
     */
    static void ExitProgram()
    {
        Console.WriteLine("Программа завершена.");
    }
}