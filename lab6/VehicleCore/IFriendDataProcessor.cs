namespace VehicleCore;

public interface IFriendDataProcessor
{
    string PluginTitle { get; }
    string PluginDescription { get; }
    byte[] EncodeBeforeSave(byte[] data);
    byte[] DecodeAfterLoad(byte[] data);
}
