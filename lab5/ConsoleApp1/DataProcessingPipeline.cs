using VehicleCore;

namespace ConsoleApp1;

public class DataProcessingPipeline
{
    private readonly IEnumerable<IDataProcessingPlugin> _plugins;

    public DataProcessingPipeline(IEnumerable<IDataProcessingPlugin> plugins) => _plugins = plugins;

    public byte[] ProcessBeforeSave(byte[] data)
    {
        byte[] result = data;
        foreach (var plugin in _plugins)
            result = plugin.ProcessBeforeSave(result);
        return result;
    }

    public byte[] ProcessAfterLoad(byte[] data)
    {
        byte[] result = data;
        foreach (var plugin in _plugins.Reverse())
            result = plugin.ProcessAfterLoad(result);
        return result;
    }
}
