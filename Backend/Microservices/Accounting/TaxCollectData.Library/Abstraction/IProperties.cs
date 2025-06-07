using TaxCollectData.Library.Enums;

namespace TaxCollectData.Library.Abstraction;

public interface IProperties
{
    string ApiVersion { get; }
    string TokenHeaderName { get; }
    Dictionary<string, string> CustomHeaders { get; }
    string GetTokenApiAddress { get; }
    string SendInvoiceApiAddress { get; }
    string GetFiscalInformationApiAddress { get; }
    string InquiryByUidApiAddress { get; }
    string InquiryByReferenceNumberApiAddress { get; }
    string InquiryByTimeApiAddress { get; }
    string InquiryByTimeRangeApiAddress { get; }
    string GetServerInformationApiAddress { get; }
    string GetServiceStuffListApiAddress { get; }
    string GetEconomicCodeInformationApiAddress { get; }
}