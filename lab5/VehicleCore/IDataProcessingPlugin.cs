namespace VehicleCore;

public interface IDataProcessingPlugin
{
    string Name { get; }
    string Description { get; }
    byte[] ProcessBeforeSave(byte[] data);
    byte[] ProcessAfterLoad(byte[] data);
}