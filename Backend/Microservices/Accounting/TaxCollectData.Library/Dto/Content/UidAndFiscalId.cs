namespace TaxCollectData.Library.Dto.Content
{
    public record UidAndFiscalId
    {
        public UidAndFiscalId(string uid, string fiscalId)
        {
            Uid = uid;
            FiscalId = fiscalId;
        }

        public string Uid { get; }

        public string FiscalId { get; }
    }
}