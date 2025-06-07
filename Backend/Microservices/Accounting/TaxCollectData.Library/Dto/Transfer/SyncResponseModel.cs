namespace TaxCollectData.Library.Dto.Transfer
{
    public record SyncResponseModel<T> : SignedRequest
    {
        public SyncResponseModel(long timestamp, ResponsePacketModel<T> result, List<ErrorModel> errors)
        {
            Timestamp = timestamp;
            Result = result;
            Errors = errors;
        }

        public long Timestamp { get; }
        public ResponsePacketModel<T> Result { get; }
        public List<ErrorModel> Errors { get; }
    }
}