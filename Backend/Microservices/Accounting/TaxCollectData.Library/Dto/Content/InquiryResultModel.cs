namespace TaxCollectData.Library.Dto.Content
{
    public record InquiryResultModel
    {
        public InquiryResultModel(string referenceNumber, string uid, string status, object data, string packetType, string fiscalId)
        {
            ReferenceNumber = referenceNumber;
            Uid = uid;
            Status = status;
            Data = data;
            PacketType = packetType;
            FiscalId = fiscalId;
        }

        public string ReferenceNumber { get; }

        public string Uid { get; }

        public string Status { get; }

        public object Data { get; }

        public string PacketType { get; }

        public string FiscalId { get; }
    }
}