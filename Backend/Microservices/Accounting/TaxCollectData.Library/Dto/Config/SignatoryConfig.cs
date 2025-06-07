namespace TaxCollectData.Library.Dto.Config;

public class SignatoryConfig
{
    public SignatoryConfig(string privateKey, string keyId)
    {
        PrivateKey = privateKey;
        KeyId = keyId;
    }

    public string PrivateKey { get; }

    public string KeyId { get; }
}