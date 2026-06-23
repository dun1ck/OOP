using VehicleCore;

namespace ConsoleApp1;

public class FriendPluginAdapter : IDataProcessingPlugin
{
    private readonly IFriendDataProcessor _friend;

    public FriendPluginAdapter(IFriendDataProcessor friend) => _friend = friend;

    public string Name => $"[Адаптер] {_friend.PluginTitle}";
    public string Description => _friend.PluginDescription;

    public byte[] ProcessBeforeSave(byte[] data) => _friend.EncodeBeforeSave(data);
    public byte[] ProcessAfterLoad(byte[] data) => _friend.DecodeAfterLoad(data);
}
