using System.Xml.Serialization;
using VehicleCore;

namespace ConsoleApp1;

public class XmlVehicleSerializer
{
    private readonly Type[] _extraTypes;

    public XmlVehicleSerializer(IEnumerable<Type> pluginVehicleTypes) =>
        _extraTypes = pluginVehicleTypes.Distinct().ToArray();

    public byte[] SerializeToBytes(List<Vehicle> vehicles)
    {
        var serializer = new XmlSerializer(typeof(List<Vehicle>), _extraTypes);
        using var ms = new MemoryStream();
        serializer.Serialize(ms, vehicles);
        return ms.ToArray();
    }

    public List<Vehicle> DeserializeFromBytes(byte[] data)
    {
        var serializer = new XmlSerializer(typeof(List<Vehicle>), _extraTypes);
        using var ms = new MemoryStream(data);
        return (List<Vehicle>)serializer.Deserialize(ms)!;
    }
}
