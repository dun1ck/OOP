using System.Xml.Serialization;
using VehicleCore;

namespace ConsoleApp1;

public class XmlVehicleSerializer
{
    private readonly Type[] _extraTypes;

    public XmlVehicleSerializer(IEnumerable<Type> pluginVehicleTypes)
    {
        _extraTypes = pluginVehicleTypes.Distinct().ToArray();
    }

    public void Serialize(List<Vehicle> vehicles, string path)
    {
        var serializer = new XmlSerializer(typeof(List<Vehicle>), _extraTypes);
        using var fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, vehicles);
    }

    public List<Vehicle> Deserialize(string path)
    {
        var serializer = new XmlSerializer(typeof(List<Vehicle>), _extraTypes);
        using var fs = new FileStream(path, FileMode.Open);
        return (List<Vehicle>)serializer.Deserialize(fs)!;
    }
}
