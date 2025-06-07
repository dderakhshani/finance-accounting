namespace TaxCollectData.Library.Dto.Transfer
{
    public record ErrorResponse : SignedRequest
    {
        public long Timestamp { get; }

        public List<ErrorModel> Errors { get; }

        public ErrorResponse(long timestamp, List<ErrorModel> errors)
        {
            Timestamp = timestamp;
            Errors = errors;
        }
    }
}