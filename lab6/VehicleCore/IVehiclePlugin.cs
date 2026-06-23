namespace VehicleCore;

public interface IVehiclePlugin
{
    string VehicleTypeName { get; }
    Type VehicleType { get; }
    Vehicle CreateVehicle();
}
