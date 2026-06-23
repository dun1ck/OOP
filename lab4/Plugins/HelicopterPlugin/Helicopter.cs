using VehicleCore;

namespace HelicopterPlugin;

public class Helicopter : Vehicle
{
    public int RotorCount { get; set; }
    public int MaxAltitude { get; set; }

    public override void DisplayInfo() =>
        Console.WriteLine($"Вертолёт | Марка: {Brand} | Год: {Year} | Винтов: {RotorCount} | Макс. высота: {MaxAltitude}м");

    public override void Edit()
    {
        Console.Write("Введите марку: ");
        Brand = Console.ReadLine() ?? string.Empty;
        Console.Write("Введите год выпуска: ");
        Year = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Введите количество винтов: ");
        RotorCount = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Введите максимальную высоту (м): ");
        MaxAltitude = int.Parse(Console.ReadLine() ?? "0");
    }
}

public class HelicopterVehiclePlugin : IVehiclePlugin
{
    public string VehicleTypeName => "Вертолёт";
    public Type VehicleType => typeof(Helicopter);
    public Vehicle CreateVehicle() => new Helicopter();
}
