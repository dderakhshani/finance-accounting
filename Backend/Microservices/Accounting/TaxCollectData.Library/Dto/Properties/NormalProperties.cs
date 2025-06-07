using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Constants;
using TaxCollectData.Library.Enums;

namespace TaxCollectData.Library.Dto.Properties;

public class NormalProperties : IProperties
{
    private readonly ClientType _clientType;
    private const string Sync = "sync";
    private const string Async = "async";

    private readonly Dictionary<ClientType, string> _clientTypes = new()
    {
        { ClientType.TSP, "tsp" },
        { ClientType.SELF_TSP, "self-tsp" }
    };

    public NormalProperties(ClientType clientType, string apiVersion = "")
    {
        _clientType = clientType;
        ApiVersion = apiVersion;
    }
    public string ApiVersion { get; }
    public string TokenHeaderName => TransferConstants.AuthorizationHeader;
    public Dictionary<string, string> CustomHeaders { get; } = new();
    public string GetTokenApiAddress => Path.Combine(_clientTypes[_clientType], ApiVersion, Sync, PacketTypeConstants.PacketTypeGetToken);
    public string SendInvoiceApiAddress => Path.Combine(_clientTypes[_clientType], ApiVersion, Async, "normal-enqueue");
    public string GetFiscalInformationApiAddress => Path.Combine(_clientTypes[_clientType], ApiVersion, Sync, PacketTypeConstants.PacketTypeGetFiscalInformation);
    public string InquiryByUidApiAddress => Path.Combine(_clientTypes[_clientType], ApiVersion, Sync, PacketTypeConstants.PacketTypeInquiryByUid);
    public string InquiryByReferenceNumberApiAddress => Path.Combine(_clientTypes[_clientType], ApiVersion, Sync, PacketTypeConstants.PacketTypeInquiryByReferenceNumber);
    public string InquiryByTimeApiAddress => Path.Combine(_clientTypes[_clientType], ApiVersion, Sync, PacketTypeConstants.PacketTypeInquiryByTime);
    public string InquiryByTimeRangeApiAddress => Path.Combine(_clientTypes[_clientType], ApiVersion, Sync, PacketTypeConstants.PacketTypeInquiryByTimeRange);
    public string GetServerInformationApiAddress => Path.Combine(_clientTypes[_clientType], ApiVersion, Sync, PacketTypeConstants.PacketTypeGetServerInformation);
    public string GetServiceStuffListApiAddress => Path.Combine(_clientTypes[_clientType], ApiVersion, Sync, PacketTypeConstants.PacketTypeGetServiceStuffList);
    public string GetEconomicCodeInformationApiAddress => Path.Combine(_clientTypes[_clientType], ApiVersion, Sync, PacketTypeConstants.PacketTypeGetEconomicCodeInformation);
}