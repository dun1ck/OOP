using VehicleCore;

namespace ConsoleApp1;

/// <summary>
/// Паттерн Strategy: цепочка взаимозаменяемых алгоритмов обработки.
/// </summary>
public class DataProcessingPipeline
{
    private readonly IReadOnlyList<IDataProcessingPlugin> _strategies;

    public DataProcessingPipeline(IEnumerable<IDataProcessingPlugin> strategies) =>
        _strategies = strategies.ToList();

    public byte[] ProcessBeforeSave(byte[] data)
    {
        byte[] result = data;
        foreach (var s in _strategies)
            result = s.ProcessBeforeSave(result);
        return result;
    }

    public byte[] ProcessAfterLoad(byte[] data)
    {
        byte[] result = data;
        foreach (var s in _strategies.Reverse())
            result = s.ProcessAfterLoad(result);
        return result;
    }
}
