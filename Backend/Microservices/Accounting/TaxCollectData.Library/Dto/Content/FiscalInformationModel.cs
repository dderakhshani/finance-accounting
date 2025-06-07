using TaxCollectData.Library.Enums;

namespace TaxCollectData.Library.Dto.Content
{
    public record FiscalInformationModel
    {
        public FiscalInformationModel(string nameTrade, FiscalStatus fiscalStatus, string economicCode, string nationalId)
        {
            NameTrade = nameTrade;
            FiscalStatus = fiscalStatus;
            EconomicCode = economicCode;
            NationalId = nationalId;
        }

        public string NameTrade { get; }

        public FiscalStatus FiscalStatus { get; }

        public string EconomicCode { get; }

        public string NationalId { get; }
    }
}