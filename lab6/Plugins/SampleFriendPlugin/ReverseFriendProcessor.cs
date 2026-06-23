using VehicleCore;
using System.Text;

namespace SampleFriendPlugin;

public class UppercaseFriendProcessor : IFriendDataProcessor
{
    public string PluginTitle => "Uppercase";
    public string PluginDescription => "Переводит текстовые данные в UPPERCASE";

    public byte[] EncodeBeforeSave(byte[] data)
    {
        string text = Encoding.UTF8.GetString(data);
        return Encoding.UTF8.GetBytes(text.ToUpperInvariant());
    }

    public byte[] DecodeAfterLoad(byte[] data)
    {
        return data;
    }
}
