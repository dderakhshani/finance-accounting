namespace TaxCollectData.Library.Dto.Transfer
{
    public record SyncRequestDto<T> : SignedRequest
    {
        public SyncRequestDto(PacketDto<T> packet) : base()
        {
            Packet = packet ?? throw new ArgumentNullException(nameof(packet));
        }

        public SyncRequestDto(string signature, string signatureKeyId, PacketDto<T> packet) : base(signature, signatureKeyId)
        {
            Packet = packet ?? throw new ArgumentNullException(nameof(packet));
        }

        public PacketDto<T> Packet { get; }
    }
}