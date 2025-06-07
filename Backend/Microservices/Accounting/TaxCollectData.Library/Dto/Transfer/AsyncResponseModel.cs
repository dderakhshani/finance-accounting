namespace TaxCollectData.Library.Dto.Transfer
{
    public record AsyncResponseModel : SignedRequest
    {
        public AsyncResponseModel(long timestamp, HashSet<PacketResponse> result, List<ErrorModel> errors)
        {
            Timestamp = timestamp;
            Result = result;
            Errors = errors;
        }

        public long Timestamp { get; }
        public HashSet<PacketResponse> Result { get; }
        public List<ErrorModel> Errors { get; }
    }
}