namespace TaxCollectData.Library.Dto.Transfer
{
    public record PacketDto<T>
    {
        public PacketDto(string uid, string packetType, string fiscalId, T data, bool retry, string encryptionKeyId, string symmetricKey, string iv, string dataSignature, string signatureKeyId)
        {
            Uid = uid;
            PacketType = packetType;
            FiscalId = fiscalId;
            Data = data;
            Retry = retry;
            EncryptionKeyId = encryptionKeyId;
            SymmetricKey = symmetricKey;
            Iv = iv;
            DataSignature = dataSignature;
            SignatureKeyId = signatureKeyId;
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

        public string SignatureKeyId { get; }
    }
}