using System.Text;
using System.Text.Json;

namespace Reaper;

public class Encryption(string key)
{
    private readonly byte[] key = Encoding.UTF8.GetBytes(key);

    public byte[] EncryptDecrypt(byte[] data)
    {
        byte[] result = new byte[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            result[i] = (byte)(data[i] ^ key[i % key.Length]);
        }
        return result;
    }

    public byte[] SerializeEncrypt(object value)
    {
        string json = JsonSerializer.Serialize(value);
        byte[] jsonData = Encoding.UTF8.GetBytes(json);
        return EncryptDecrypt(jsonData);
    }

    public T DecryptDeserialize<T>(byte[] data)
    {
        byte[] decrypted = EncryptDecrypt(data);
        string json = Encoding.UTF8.GetString(decrypted);
        return JsonSerializer.Deserialize<T>(json);
    }
}
