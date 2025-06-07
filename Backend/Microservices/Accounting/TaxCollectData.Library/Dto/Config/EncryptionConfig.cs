namespace TaxCollectData.Library.Dto.Config;

public class EncryptionConfig
{
    internal EncryptionConfig()
    {
    }

    public EncryptionConfig(string taxOrgPublicKey, string encryptionKeyId)
    {
        TaxOrgPublicKey = taxOrgPublicKey;
        EncryptionKeyId = encryptionKeyId;
    }

    public string TaxOrgPublicKey { get; internal set; }
    public string EncryptionKeyId { get; internal set; }
}