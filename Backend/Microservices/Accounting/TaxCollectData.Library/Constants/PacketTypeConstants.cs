namespace TaxCollectData.Library.Constants;

internal static class PacketTypeConstants
{
    public const string InvoiceV01 = "INVOICE.V01";
    public const string PacketTypeGetToken = "GET_TOKEN";
    public const string PacketTypeGetServerInformation = "GET_SERVER_INFORMATION";
    public const string PacketTypeInquiryByUid = "INQUIRY_BY_UID";
    public const string PacketTypeInquiryByTimeRange = "INQUIRY_BY_TIME_RANGE";
    public const string PacketTypeInquiryByTime = "INQUIRY_BY_TIME";
    public const string PacketTypeInquiryByReferenceNumber = "INQUIRY_BY_REFERENCE_NUMBER";
    public const string PacketTypeGetServiceStuffList = "GET_SERVICE_STUFF_LIST";
    public const string PacketTypeGetEconomicCodeInformation = "GET_ECONOMIC_CODE_INFORMATION";
    public const string PacketTypeGetFiscalFullInformation = "GET_FISCAL_FULL_INFORMATION";
    public const string PacketTypeGetFiscalInformation = "GET_FISCAL_INFORMATION";
    public const string PacketTypeReceiveInvoiceConfirm = "RECEIVE_INVOICE_CONFIRM";
}