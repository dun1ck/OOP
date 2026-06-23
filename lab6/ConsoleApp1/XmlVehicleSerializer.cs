using System.Xml.Serialization;
using VehicleCore;

namespace ConsoleApp1;

public class XmlVehicleSerializer
{
    private readonly Type[] _extraTypes;
    public XmlVehicleSerializer(IEnumerable<Type> types) => _extraTypes = types.Distinct().ToArray();

    public byte[] SerializeToBytes(List<Vehicle> vehicles)
    {
        var ser = new XmlSerializer(typeof(List<Vehicle>), _extraTypes);
        using var ms = new MemoryStream();
        ser.Serialize(ms, vehicles);
        return ms.ToArray();
    }

    public List<Vehicle> DeserializeFromBytes(byte[] data)
    {
        var ser = new XmlSerializer(typeof(List<Vehicle>), _extraTypes);
        using var ms = new MemoryStream(data);
        return (List<Vehicle>)ser.Deserialize(ms)!;
    }
}
