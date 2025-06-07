namespace TaxCollectData.Library.Dto.Content
{
    public record InquiryByTimeDto
    {
        public InquiryByTimeDto(string time)
        {
            Time = time;
        }

        public string Time { get; }
    }
}