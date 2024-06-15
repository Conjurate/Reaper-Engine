using System.Text;
using System.Text.Json;

namespace Reaper.Persistence;

public class FileDataHandler<T>
{
    private const string encryptionWord = "SreaperS";

    public bool HasEncryption => crypto != null;

    private string filePath;
    private string fileName;
    private Encryption crypto;

    public FileDataHandler(string filePath, string fileName, bool useEncryption)
    {
        this.filePath = filePath;
        this.fileName = fileName;

        if (useEncryption)
        {
            crypto = new Encryption(encryptionWord);
        }
    }

    public T Load()
    {
        string fullPath = Path.Combine(filePath, fileName);
        T loadedFile = default;
        if (File.Exists(fullPath))
        {
            try
            {
                byte[] data = File.ReadAllBytes(fullPath);

                if (HasEncryption)
                {
                    data = crypto.EncryptDecrypt(data);
                }

                loadedFile = JsonSerializer.Deserialize<T>(data);
            }
            catch (Exception e)
            {
                Log.Error("An error occured while loading a file: " + fullPath + "\n" + e);
            }
        }

        return loadedFile;
    }

    public void Save(T file)
    {
        string fullPath = Path.Combine(filePath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(file));

            if (HasEncryption)
            {
                data = crypto.EncryptDecrypt(data);
            }

            File.WriteAllBytes(fullPath, data);
        }
        catch (Exception e)
        {
            Log.Error("An error occured while saving to file: " + fullPath + "\n" + e);
        }
    }
}
