namespace TaxCollectData.Library.Dto.Config;

public class Pkcs8SignatoryConfig
{
    public Pkcs8SignatoryConfig(string pkcs8PrivateKeyFileAddress, string keyId)
    {
        Pkcs8PrivateKeyFileAddress = pkcs8PrivateKeyFileAddress;
        KeyId = keyId;
    }

    public string Pkcs8PrivateKeyFileAddress { get; }

    public string KeyId { get; }
}