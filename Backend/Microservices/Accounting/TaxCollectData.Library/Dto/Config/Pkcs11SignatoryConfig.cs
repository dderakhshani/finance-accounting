namespace TaxCollectData.Library.Dto.Config;

public class Pkcs11SignatoryConfig
{
    public Pkcs11SignatoryConfig(string keyId, string userPin, string libraryPath)
    {
        KeyId = keyId;
        UserPin = userPin;
        LibraryPath = libraryPath;
    }

    public string KeyId { get; }
    public string UserPin { get; }
    public string LibraryPath { get; }
}