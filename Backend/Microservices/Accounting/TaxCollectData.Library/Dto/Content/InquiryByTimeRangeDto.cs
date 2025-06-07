namespace TaxCollectData.Library.Dto.Content
{
    public record InquiryByTimeRangeDto
    {
        public InquiryByTimeRangeDto(string startDate, string endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public string StartDate { get; }

        public string EndDate { get; }
    }
}