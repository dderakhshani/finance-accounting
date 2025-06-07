namespace TaxCollectData.Library.Dto.Content
{
    public record InquiryByReferenceNumberDto
    {
        public InquiryByReferenceNumberDto(List<string> referenceNumber)
        {
            ReferenceNumber = referenceNumber;
        }

        public List<string> ReferenceNumber { get; }
    }
}