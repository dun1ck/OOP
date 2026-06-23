namespace EncryptionPlugin;
using VehicleCore;

public class EncryptionProcessingPlugin : IDataProcessingPlugin
{
    private const string Magic = "LAB5ENC1";
    private const byte Key = 42;  // Один байт для простоты

    public string Name => "Шифрование XOR";
    public string Description => "Шифрует данные перед сохранением и расшифровывает после загрузки";

    public byte[] ProcessBeforeSave(byte[] data)
    {
        // Шифруем каждый байт
        byte[] encrypted = new byte[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            encrypted[i] = (byte)(data[i] ^ Key);  // XOR с ключом
        }

        // Добавляем сигнатуру в начало
        byte[] magic = System.Text.Encoding.UTF8.GetBytes(Magic);
        byte[] result = new byte[magic.Length + encrypted.Length];
        Array.Copy(magic, 0, result, 0, magic.Length);
        Array.Copy(encrypted, 0, result, magic.Length, encrypted.Length);
        return result;
    }

    public byte[] ProcessAfterLoad(byte[] data)
    {
        byte[] magic = System.Text.Encoding.UTF8.GetBytes(Magic);

        // Проверяем сигнатуру
        if (data.Length <= magic.Length)
            return data;

        for (int i = 0; i < magic.Length; i++)
        {
            if (data[i] != magic[i])
                return data;  // Нет сигнатуры — данные не шифрованы
        }

        // Извлекаем зашифрованную часть
        byte[] payload = new byte[data.Length - magic.Length];
        Array.Copy(data, magic.Length, payload, 0, payload.Length);

        // Расшифровываем (XOR сам себе обратный)
        byte[] decrypted = new byte[payload.Length];
        for (int i = 0; i < payload.Length; i++)
        {
            decrypted[i] = (byte)(payload[i] ^ Key);  // Тот же XOR
        }

        return decrypted;
    }
}