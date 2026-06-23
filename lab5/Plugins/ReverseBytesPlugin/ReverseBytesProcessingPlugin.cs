using VehicleCore;

namespace ReverseBytesPlugin;

public class ReverseBytesProcessingPlugin : IDataProcessingPlugin
{
    public string Name => "Реверс данных";
    public string Description => "Перед сохранением разворачивает байты; после загрузки выполняет обратную операцию.";

    public byte[] ProcessBeforeSave(byte[] data) => data.Reverse().ToArray();
    public byte[] ProcessAfterLoad(byte[] data) => data.Reverse().ToArray();
}