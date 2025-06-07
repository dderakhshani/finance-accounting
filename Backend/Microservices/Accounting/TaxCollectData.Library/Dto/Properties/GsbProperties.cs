using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Enums;

namespace TaxCollectData.Library.Dto.Properties;

public class GsbProperties : IProperties
{
    private readonly ClientType _clientType;
    private readonly Dictionary<ClientType, string> _clientTypes = new()
    {
        { ClientType.TSP, "Tsp" },
        { ClientType.SELF_TSP, "SelfTsp" }
    };

    public GsbProperties(ClientType clientType, string tokenHeaderName, Dictionary<string, string> customHeaders = null)
    {
        _clientType = clientType;
        TokenHeaderName = tokenHeaderName;
        CustomHeaders = customHeaders ?? new();
    }
    
    public string ApiVersion => string.Empty;
    public string TokenHeaderName { get; }
    public Dictionary<string, string> CustomHeaders { get; }
    public string GetTokenApiAddress => $"{_clientTypes[_clientType]}GetToken";
    public string SendInvoiceApiAddress => $"{_clientTypes[_clientType]}NormalEnqueue";
    public string GetFiscalInformationApiAddress => $"{_clientTypes[_clientType]}GetFiscalInformation";
    public string InquiryByUidApiAddress => $"{_clientTypes[_clientType]}InquiryByUid";
    public string InquiryByReferenceNumberApiAddress => $"{_clientTypes[_clientType]}InquiryByReferenceNumber";
    public string InquiryByTimeApiAddress => $"{_clientTypes[_clientType]}InquiryByTime";
    public string InquiryByTimeRangeApiAddress => $"{_clientTypes[_clientType]}GetServiceStuffList";
    public string GetServerInformationApiAddress => $"{_clientTypes[_clientType]}GETServerInformation";
    public string GetServiceStuffListApiAddress => $"{_clientTypes[_clientType]}GetServiceStuffList";
    public string GetEconomicCodeInformationApiAddress => $"{_clientTypes[_clientType]}GetEconmicCodeInformation";
}