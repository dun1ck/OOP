using System.Xml.Serialization;

namespace VehicleCore;

[XmlInclude(typeof(Car))]
[XmlInclude(typeof(Truck))]
[XmlInclude(typeof(Motorcycle))]
[XmlInclude(typeof(Bus))]
[XmlInclude(typeof(ElectricCar))]
[XmlInclude(typeof(SportCar))]
public abstract class Vehicle
{
    public string Brand { get; set; } = string.Empty;
    public int Year { get; set; }
    public abstract void DisplayInfo();
    public abstract void Edit();
}

public class Car : Vehicle
{
    public int Doors { get; set; }

    public override void DisplayInfo() =>
        Console.WriteLine($"Легковой автомобиль | Марка: {Brand} | Год: {Year} | Дверей: {Doors}");

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine() ?? string.Empty;
        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Введите количество дверей: ");
        Doors = int.Parse(Console.ReadLine() ?? "0");
    }
}

public class Truck : Vehicle
{
    public double LoadCapacity { get; set; }

    public override void DisplayInfo() =>
        Console.WriteLine($"Грузовик | Марка: {Brand} | Год: {Year} | Грузоподъемность: {LoadCapacity}");

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine() ?? string.Empty;
        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Введите грузоподъемность: ");
        LoadCapacity = double.Parse(Console.ReadLine() ?? "0");
    }
}

public class Motorcycle : Vehicle
{
    public bool HasSidecar { get; set; }

    public override void DisplayInfo() =>
        Console.WriteLine($"Мотоцикл | Марка: {Brand} | Год: {Year} | Люлька: {HasSidecar}");

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine() ?? string.Empty;
        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Есть ли люлька (true/false): ");
        HasSidecar = bool.Parse(Console.ReadLine() ?? "false");
    }
}

public class Bus : Vehicle
{
    public int PassengerCapacity { get; set; }

    public override void DisplayInfo() =>
        Console.WriteLine($"Автобус | Марка: {Brand} | Год: {Year} | Вместимость: {PassengerCapacity}");

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine() ?? string.Empty;
        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Введите пассажировместимость: ");
        PassengerCapacity = int.Parse(Console.ReadLine() ?? "0");
    }
}

public class ElectricCar : Vehicle
{
    public int BatteryCapacity { get; set; }

    public override void DisplayInfo() =>
        Console.WriteLine($"Электромобиль | Марка: {Brand} | Год: {Year} | Батарея: {BatteryCapacity}");

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine() ?? string.Empty;
        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Введите емкость батареи: ");
        BatteryCapacity = int.Parse(Console.ReadLine() ?? "0");
    }
}

public class SportCar : Vehicle
{
    public int MaxSpeed { get; set; }

    public override void DisplayInfo() =>
        Console.WriteLine($"Спорткар | Марка: {Brand} | Год: {Year} | Максимальная скорость: {MaxSpeed}");

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine() ?? string.Empty;
        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Введите максимальную скорость: ");
        MaxSpeed = int.Parse(Console.ReadLine() ?? "0");
    }
}
