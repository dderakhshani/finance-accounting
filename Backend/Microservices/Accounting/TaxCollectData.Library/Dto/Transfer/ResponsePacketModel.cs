namespace TaxCollectData.Library.Dto.Transfer
{
    public record ResponsePacketModel<T>
    {
        public ResponsePacketModel(string uid, string packetType, T data, string encryptionKeyId, string symmetricKey, string iv)
        {
            Uid = uid;
            PacketType = packetType;
            Data = data;
            EncryptionKeyId = encryptionKeyId;
            SymmetricKey = symmetricKey;
            Iv = iv;
        }

        public string Uid { get; }
        public string PacketType { get; }
        public T Data { get; }
        public string EncryptionKeyId { get; }
        public string SymmetricKey { get; }
        public string Iv { get; }
    }
}