namespace TaxCollectData.Library.Dto.Transfer
{
    public record ErrorModel
    {
        public ErrorModel(string detail, string errorCode)
        {
            Detail = detail;
            ErrorCode = errorCode;
        }

        public string Detail { get; }

        public string ErrorCode { get; }
    }
}