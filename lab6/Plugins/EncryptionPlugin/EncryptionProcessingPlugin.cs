using System.Security.Cryptography;
using System.Text;
using VehicleCore;

namespace EncryptionPlugin;

public class EncryptionProcessingPlugin : IDataProcessingPlugin
{
    private const string Magic = "LAB6ENC1";
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("OOPLab6Key123456");
    private static readonly byte[] Iv = Encoding.UTF8.GetBytes("OOPLab6IV1234567");

    public string Name => "Шифрование AES";
    public string Description => "Шифрование/дешифрование (вариант 3)";

    public byte[] ProcessBeforeSave(byte[] data)
    {
        using var aes = Aes.Create();
        aes.Key = Key; aes.IV = Iv;
        byte[] enc = aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
        return Encoding.UTF8.GetBytes(Magic).Concat(enc).ToArray();
    }

    public byte[] ProcessAfterLoad(byte[] data)
    {
        byte[] magic = Encoding.UTF8.GetBytes(Magic);
        if (data.Length <= magic.Length || !data.AsSpan(0, magic.Length).SequenceEqual(magic))
            return data;
        byte[] payload = data[magic.Length..];
        using var aes = Aes.Create();
        aes.Key = Key; aes.IV = Iv;
        return aes.CreateDecryptor().TransformFinalBlock(payload, 0, payload.Length);
    }
}
