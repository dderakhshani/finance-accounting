namespace TaxCollectData.Library.Dto;

public record PacketDtoWithoutSignatureKeyId<T>
{
    public PacketDtoWithoutSignatureKeyId(string uid, string packetType, bool retry, T data, string encryptionKeyId, string symmetricKey, string iv, string fiscalId, string dataSignature)
    {
        Uid = uid;
        PacketType = packetType;
        Retry = retry;
        Data = data;
        EncryptionKeyId = encryptionKeyId;
        SymmetricKey = symmetricKey;
        Iv = iv;
        FiscalId = fiscalId;
        DataSignature = dataSignature;
    }

    public string Uid { get; }

    public string PacketType { get; }

    public bool Retry { get; }

    public T Data { get; }

    public string EncryptionKeyId { get; }

    public string SymmetricKey { get; }

    public string Iv { get; }

    public string FiscalId { get; }

    public string DataSignature { get; }
}