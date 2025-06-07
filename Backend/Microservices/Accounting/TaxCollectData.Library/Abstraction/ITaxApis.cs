using TaxCollectData.Library.Dto;
using TaxCollectData.Library.Dto.Content;
using TaxCollectData.Library.Dto.Transfer;

namespace TaxCollectData.Library.Abstraction
{
    public interface ITaxApis
    {
        TokenModel? RequestToken();
        Task<TokenModel?> RequestTokenAsync();
        TokenModel? GetToken();
        void SetToken(TokenModel tokenModel);
        HttpResponse<AsyncResponseModel?>? SendInvoices(List<InvoiceDto> invoices, Dictionary<string, string>? headers);
        Task<HttpResponse<AsyncResponseModel?>?> SendInvoicesAsync(List<InvoiceDto> invoices, Dictionary<string, string>? headers);
        HttpResponse<AsyncResponseModel?>? SendTspInvoices(List<InvoiceDtoWrapper> invoices, Dictionary<string, string>? headers);
        Task<HttpResponse<AsyncResponseModel?>?> SendTspInvoicesAsync(List<InvoiceDtoWrapper> invoices, Dictionary<string, string>? headers);
        ServerInformationModel? GetServerInformation();
        Task<ServerInformationModel?> GetServerInformationAsync();
        List<InquiryResultModel>? InquiryByUidAndFiscalId(List<UidAndFiscalId> uidAndFiscalIds);
        Task<List<InquiryResultModel>?> InquiryByUidAndFiscalIdAsync(List<UidAndFiscalId> uidAndFiscalIds);
        List<InquiryResultModel>? InquiryByTime(string persianTime);
        Task<List<InquiryResultModel>?> InquiryByTimeAsync(string persianTime);
        List<InquiryResultModel>? InquiryByTimeRange(string startDatePersian, string toDatePersian);
        Task<List<InquiryResultModel>?> InquiryByTimeRangeAsync(string startDatePersian, string toDatePersian);
        List<InquiryResultModel>? InquiryByReferenceId(List<string> referenceIds);
        Task<List<InquiryResultModel>?> InquiryByReferenceIdAsync(List<string> referenceIds);
        FiscalInformationModel? GetFiscalInformation(string fiscalId);
        Task<FiscalInformationModel?> GetFiscalInformationAsync(string fiscalId);
        SearchResultModel<ServiceStuffModel>? GetServiceStuffList(SearchDto searchDto);
        Task<SearchResultModel<ServiceStuffModel>?> GetServiceStuffListAsync(SearchDto searchDto);
        EconomicCodeModel? GetEconomicCodeInformation(string economicCode);
        Task<EconomicCodeModel?> GetEconomicCodeInformationAsync(string economicCode);
    }
}