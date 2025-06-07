namespace TaxCollectData.Library.Dto.Transfer
{
    public record PacketResponse
    {
        public PacketResponse(string uid, string referenceNumber, string errorCode, string errorDetail)
        {
            Uid = uid;
            ReferenceNumber = referenceNumber;
            ErrorCode = errorCode;
            ErrorDetail = errorDetail;
        }

        public string Uid { get; }

        public string ReferenceNumber { get; }

        public string ErrorCode { get; }

        public string ErrorDetail { get; }
    }
}