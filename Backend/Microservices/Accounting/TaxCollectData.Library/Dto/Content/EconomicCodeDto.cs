namespace TaxCollectData.Library.Dto.Content
{
    public record EconomicCodeDto
    {
        public EconomicCodeDto(string economicCode)
        {
            EconomicCode = economicCode;
        }

        public string EconomicCode { get; }
    }
}