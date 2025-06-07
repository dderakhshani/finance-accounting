namespace TaxCollectData.Library.Dto.Transfer
{
    public record AsyncRequestDto<T> : SignedRequest
    {
        public List<PacketDto<T>> Packets { get; }

        public AsyncRequestDto(List<PacketDto<T>> packets)
        {
            Packets = packets ?? throw new ArgumentNullException(nameof(packets));
        }

        public AsyncRequestDto(string signature, string signatureKeyId, List<PacketDto<T>> packets) : base(signature, signatureKeyId)
        {
            Packets = packets ?? throw new ArgumentNullException(nameof(packets));
        }
    }
}