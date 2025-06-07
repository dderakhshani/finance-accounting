using TaxCollectData.Library.Enums;

namespace TaxCollectData.Library.Dto.Content
{
    public record FiscalFullInformationModel
    {
        public FiscalFullInformationModel(string nameTrade, FiscalStatus fiscalStatus, decimal saleThreshold,
         string economicCode, string nationalId)
        {
            NameTrade = nameTrade;
            FiscalStatus = fiscalStatus;
            SaleThreshold = saleThreshold;
            EconomicCode = economicCode;
            NationalId = nationalId;
        }

        public string NameTrade { get; }

        public FiscalStatus FiscalStatus { get; }

        public decimal SaleThreshold { get; }

        public string EconomicCode { get; }

        public string NationalId { get; }
    }
}