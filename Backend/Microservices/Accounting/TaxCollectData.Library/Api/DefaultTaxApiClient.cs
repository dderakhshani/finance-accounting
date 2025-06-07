using System.Text.Json;
using Microsoft.VisualStudio.Threading;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Constants;
using TaxCollectData.Library.Dto;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using TaxCollectData.Library.Dto.Transfer;
using TaxCollectData.Library.Exceptions;

namespace TaxCollectData.Library.Api
{
    internal class DefaultTaxApiClient : ITaxApis
    {
        private readonly ITransferApi _transferApi;
        private readonly string _clientId;
        private readonly EncryptionConfig _encryptionConfig;
        private readonly IProperties _properties;
        private TokenModel? token;

        public DefaultTaxApiClient(ITransferApi transferApi,
            string clientId,
            EncryptionConfig encryptionConfig,
            IProperties properties)
        {
            _transferApi = transferApi;
            _clientId = clientId;
            _encryptionConfig = encryptionConfig;
            _properties = properties;
        }

        public void SetToken(TokenModel tokenModel)
        {
            token = tokenModel;
        }

        public TokenModel? GetToken()
        {
            return token;
        }

        public TokenModel? RequestToken()
        {
            var data = new GetTokenDto(_clientId);
            var packetDto = PacketDtoBuilder(data, PacketTypeConstants.PacketTypeGetToken);
            var responseData = GetPacketResponse<GetTokenDto, TokenModel>(packetDto,
                new(_properties.CustomHeaders),
                _properties.GetTokenApiAddress,
                false,
                false);

            return token = responseData;
        }
        
        public async Task<TokenModel?> RequestTokenAsync()
        {
            var data = new GetTokenDto(_clientId);
            var packetDto = PacketDtoBuilder(data, PacketTypeConstants.PacketTypeGetToken);
            var responseData = await GetPacketResponseAsync<GetTokenDto, TokenModel>(packetDto, 
                    new(_properties.CustomHeaders),
                    _properties.GetTokenApiAddress,
                    false, 
                    false)
                .ConfigureAwait(false);

            return token = responseData;
        }

        public HttpResponse<AsyncResponseModel?>? SendInvoices(List<InvoiceDto> invoices, Dictionary<string, string>? headers)
        {
            using var taskContext = new JoinableTaskContext();
            var joinableTaskFactory = new JoinableTaskFactory(taskContext);
            var packets = invoices.Select(invoiceDto => PacketDtoBuilder(invoiceDto, PacketTypeConstants.InvoiceV01)).ToList();
            return joinableTaskFactory.Run(async () => await SendInvoicesAsync(invoices, headers).ConfigureAwait(false));
        }
        
        public async Task<HttpResponse<AsyncResponseModel?>?> SendInvoicesAsync(List<InvoiceDto> invoices, Dictionary<string, string>? headers)
        {
            var packets = invoices.Select(invoiceDto => PacketDtoBuilder(invoiceDto, PacketTypeConstants.InvoiceV01)).ToList();
            return await _transferApi.SendPacketsAsync(packets, 
                    _properties.SendInvoiceApiAddress, 
                    GetHeaders(headers), 
                    true, 
                    true)
                .ConfigureAwait(false);
        }

        public HttpResponse<AsyncResponseModel?>? SendTspInvoices(List<InvoiceDtoWrapper> invoices, Dictionary<string, string>? headers)
        {
            using var taskContext = new JoinableTaskContext();
            var joinableTaskFactory = new JoinableTaskFactory(taskContext);
            var packets = invoices.Select(invoiceDtoWrapper => PacketDtoBuilder(invoiceDtoWrapper.Invoice,
                PacketTypeConstants.InvoiceV01,
                invoiceDtoWrapper.Uid,
                invoiceDtoWrapper.FiscalId)).ToList();
            return joinableTaskFactory.Run(async () => await SendTspInvoicesAsync(invoices, headers).ConfigureAwait(false));
        }

        public async Task<HttpResponse<AsyncResponseModel?>?> SendTspInvoicesAsync(List<InvoiceDtoWrapper> invoices, Dictionary<string, string>? headers)
        {
            var packets = invoices.Select(invoiceDtoWrapper => PacketDtoBuilder(invoiceDtoWrapper.Invoice,
                PacketTypeConstants.InvoiceV01,
                invoiceDtoWrapper.Uid,
                invoiceDtoWrapper.FiscalId)).ToList();
            return await _transferApi.SendPacketsAsync(packets,
                    _properties.SendInvoiceApiAddress,
                    GetHeaders(headers),
                    true,
                    true)
                .ConfigureAwait(false);
        }

        public ServerInformationModel? GetServerInformation()
        {
            var packetDto = PacketDtoBuilder<object>(null, PacketTypeConstants.PacketTypeGetServerInformation);
            var responseData = GetPacketResponse<object, ServerInformationModel>(packetDto, 
                new(_properties.CustomHeaders),
                _properties.GetServerInformationApiAddress,
                false, 
                false);
            SetEncryptionConfig(responseData);
            return responseData;
        }

        public async Task<ServerInformationModel?> GetServerInformationAsync()
        {
            var packetDto = PacketDtoBuilder<object>(null, PacketTypeConstants.PacketTypeGetServerInformation);
            var responseData = await GetPacketResponseAsync<object, ServerInformationModel>(packetDto, 
                    new(_properties.CustomHeaders),
                    _properties.GetServerInformationApiAddress,
                    false, 
                    false)
                .ConfigureAwait(false);
            SetEncryptionConfig(responseData);
            return responseData;
        }

        public List<InquiryResultModel>? InquiryByUidAndFiscalId(List<UidAndFiscalId> uidAndFiscalIds)
        {
            var packetDto = PacketDtoBuilder(uidAndFiscalIds, PacketTypeConstants.PacketTypeInquiryByUid);
            var result = GetPacketResponse<List<UidAndFiscalId>, List<InquiryResultModel>>(packetDto,
                GetHeaders(),
                _properties.InquiryByUidApiAddress,
                false,
                false);
            return GetInquiryResultModels(result);

        }
        
        public async Task<List<InquiryResultModel>?> InquiryByUidAndFiscalIdAsync(List<UidAndFiscalId> uidAndFiscalIds)
        {
            var packetDto = PacketDtoBuilder(uidAndFiscalIds, PacketTypeConstants.PacketTypeInquiryByUid);
            var result = await GetPacketResponseAsync<List<UidAndFiscalId>, List<InquiryResultModel>>(packetDto, 
                    GetHeaders(), 
                    _properties.InquiryByUidApiAddress,
                    false, 
                    false)
                .ConfigureAwait(false);
            return GetInquiryResultModels(result);
        }

        public List<InquiryResultModel>? InquiryByTime(string persianTime)
        {
            var data = new InquiryByTimeDto(persianTime);
            var packetDto = PacketDtoBuilder(data, PacketTypeConstants.PacketTypeInquiryByTime);
            var result = GetPacketResponse<InquiryByTimeDto, List<InquiryResultModel>>(packetDto,
                GetHeaders(),
                _properties.InquiryByTimeApiAddress,
                false,
                false);
            return GetInquiryResultModels(result);
        }
        
        public async Task<List<InquiryResultModel>?> InquiryByTimeAsync(string persianTime)
        {
            var data = new InquiryByTimeDto(persianTime);
            var packetDto = PacketDtoBuilder(data, PacketTypeConstants.PacketTypeInquiryByTime);
            var result = await GetPacketResponseAsync<InquiryByTimeDto, List<InquiryResultModel>>(packetDto, 
                    GetHeaders(), 
                    _properties.InquiryByTimeApiAddress,
                    false, 
                    false)
                .ConfigureAwait(false);
            return GetInquiryResultModels(result);
        }

        public List<InquiryResultModel>? InquiryByTimeRange(string startDatePersian, string toDatePersian)
        {
            var data = new InquiryByTimeRangeDto(startDatePersian, toDatePersian);
            var packetDto = PacketDtoBuilder(data, PacketTypeConstants.PacketTypeInquiryByTimeRange);
            var result = GetPacketResponse<InquiryByTimeRangeDto, List<InquiryResultModel>>(packetDto, 
                GetHeaders(), 
                _properties.InquiryByTimeRangeApiAddress,
                false,
                false);
            return GetInquiryResultModels(result);
        }
        
        public async Task<List<InquiryResultModel>?> InquiryByTimeRangeAsync(string startDatePersian, string toDatePersian)
        {
            var data = new InquiryByTimeRangeDto(startDatePersian, toDatePersian);
            var packetDto = PacketDtoBuilder(data, PacketTypeConstants.PacketTypeInquiryByTimeRange);
            var result = await GetPacketResponseAsync<InquiryByTimeRangeDto, List<InquiryResultModel>>(packetDto, 
                    GetHeaders(), 
                    _properties.InquiryByTimeRangeApiAddress,
                    false, 
                    false)
                .ConfigureAwait(false);
            return GetInquiryResultModels(result);
        }

        public List<InquiryResultModel>? InquiryByReferenceId(List<string> referenceIds)
        {
            var data = new InquiryByReferenceNumberDto(referenceIds);
            var packetDto = PacketDtoBuilder(data, PacketTypeConstants.PacketTypeInquiryByReferenceNumber);
            var result = GetPacketResponse<InquiryByReferenceNumberDto, List<InquiryResultModel>>(packetDto, 
                GetHeaders(), 
                _properties.InquiryByReferenceNumberApiAddress,
                false,
                false);
            return GetInquiryResultModels(result);
        }
        
        public async Task<List<InquiryResultModel>?> InquiryByReferenceIdAsync(List<string> referenceIds)
        {
            var data = new InquiryByReferenceNumberDto(referenceIds);
            var packetDto = PacketDtoBuilder(data, PacketTypeConstants.PacketTypeInquiryByReferenceNumber);
            var result = await GetPacketResponseAsync<InquiryByReferenceNumberDto, List<InquiryResultModel>>(packetDto, 
                    GetHeaders(), 
                    _properties.InquiryByReferenceNumberApiAddress,
                    false, 
                    false)
                .ConfigureAwait(false);
            return GetInquiryResultModels(result);
        }

        public FiscalInformationModel? GetFiscalInformation(string fiscalId)
        {
            var packetDto = PacketDtoBuilder(fiscalId, PacketTypeConstants.PacketTypeGetFiscalInformation);
            return GetPacketResponse<string, FiscalInformationModel>(packetDto,
                GetHeaders(),
                _properties.GetFiscalInformationApiAddress,
                false,
                false);
        }
        
        public async Task<FiscalInformationModel?> GetFiscalInformationAsync(string fiscalId)
        {
            var packetDto = PacketDtoBuilder(fiscalId, PacketTypeConstants.PacketTypeGetFiscalInformation, fiscalId: fiscalId);
            return await GetPacketResponseAsync<string, FiscalInformationModel>(packetDto, 
                    GetHeaders(), 
                    _properties.GetFiscalInformationApiAddress,
                    false, 
                    false)
                .ConfigureAwait(false);
        }

        public SearchResultModel<ServiceStuffModel>? GetServiceStuffList(SearchDto searchDto)
        {
            var packetDto = PacketDtoBuilder(searchDto, PacketTypeConstants.PacketTypeGetServiceStuffList);
            return GetPacketResponse<SearchDto, SearchResultModel<ServiceStuffModel>>(packetDto, 
                GetHeaders(), 
                _properties.GetServiceStuffListApiAddress, 
                false, 
                true);
        }
        
        public async Task<SearchResultModel<ServiceStuffModel>?> GetServiceStuffListAsync(SearchDto searchDto)
        {
            var packetDto = PacketDtoBuilder(searchDto, PacketTypeConstants.PacketTypeGetServiceStuffList);
            return await GetPacketResponseAsync<SearchDto, SearchResultModel<ServiceStuffModel>>(packetDto, 
                    GetHeaders(), 
                    _properties.GetServiceStuffListApiAddress, 
                    false, 
                    true)
                .ConfigureAwait(false);
        }

        public EconomicCodeModel? GetEconomicCodeInformation(string economicCode)
        {
            var data = new EconomicCodeDto(economicCode);
            var packetDto = PacketDtoBuilder(data, PacketTypeConstants.PacketTypeGetEconomicCodeInformation);
            return GetPacketResponse<EconomicCodeDto, EconomicCodeModel>(packetDto,
                GetHeaders(),
                _properties.GetEconomicCodeInformationApiAddress,
                false,
                false);
        }
        
        public async Task<EconomicCodeModel?> GetEconomicCodeInformationAsync(string economicCode)
        {
            var data = new EconomicCodeDto(economicCode);
            var packetDto = PacketDtoBuilder(data, PacketTypeConstants.PacketTypeGetEconomicCodeInformation);
            return await GetPacketResponseAsync<EconomicCodeDto, EconomicCodeModel>(packetDto, 
                    GetHeaders(), 
                    _properties.GetEconomicCodeInformationApiAddress,
                    false, 
                    false)
                .ConfigureAwait(false);
        }
        
        private void SetEncryptionConfig(ServerInformationModel? responseData)
        {
            if (responseData?.PublicKeys == null || !responseData.PublicKeys.Any())
            {
                return;
            }

            var key = responseData.PublicKeys.First();
            _encryptionConfig.TaxOrgPublicKey = key.Key;
            _encryptionConfig.EncryptionKeyId = key.Id;
        }
        
        private Dictionary<string, string> GetHeaders(Dictionary<string, string>? headers = null)
        {
            headers ??= new Dictionary<string, string>();
            if (token != null)
            {
                var prefix = _properties.TokenHeaderName.Equals(TransferConstants.AuthorizationHeader, StringComparison.InvariantCultureIgnoreCase)
                    ? "Bearer "
                    : string.Empty;
                headers[_properties.TokenHeaderName] = $"{prefix}{token.Token}";
            }
            foreach (var keyValuePair in _properties.CustomHeaders)
            {
                headers[keyValuePair.Key] = keyValuePair.Value;
            }
            
            return headers;
        }

        private PacketDto<T> PacketDtoBuilder<T>(T data, string packetType, string? uid = null, string? fiscalId = null)
        {
            if (!(fiscalId is not null && fiscalId.Length > 0))
            {
                fiscalId = _clientId;
            }
            if (!(uid is not null && uid.Length > 0))
            {
                uid = Guid.NewGuid().ToString();
            }
            return new PacketDto<T>(uid,
                packetType,
                fiscalId,
                data,
                false,
                null,
                null,
                null,
                null,
                null);
        }

        private async Task<TResponse?> GetPacketResponseAsync<TRequest, TResponse>(PacketDto<TRequest> packet,
            Dictionary<string, string> headers,
            string url,
            bool encrypt,
            bool sign,
            bool returnNullIfResponseIsNull = false) where TResponse : class
        {
            var response = await _transferApi.SendPacketAsync<TRequest, TResponse>(packet, url, headers, encrypt, sign).ConfigureAwait(false);
            if (returnNullIfResponseIsNull && response == null)
            {
                return null;
            }

            if (response?.Body?.Errors != null && response.Body.Errors.Any())
            {
                throw new TaxApiException(response.Body.Errors[0].Detail);
            }

            return response?.Body?.Result.Data;
        }

        private TResponse? GetPacketResponse<TRequest, TResponse>(PacketDto<TRequest> packet,
            Dictionary<string, string> headers,
            string url,
            bool encrypt,
            bool sign,
            bool returnNullIfResponseIsNull = false) where TResponse : class
        {
            using var taskContext = new JoinableTaskContext();
            var joinableTaskFactory = new JoinableTaskFactory(taskContext);
            return joinableTaskFactory.Run(async () => await GetPacketResponseAsync<TRequest, TResponse>(packet,
                    headers,
                    url,
                    encrypt,
                    sign,
                    returnNullIfResponseIsNull)
                .ConfigureAwait(true));
        }

        private List<InquiryResultModel> GetInquiryResultModels(List<InquiryResultModel>? resultModels)
        {
            var result = new List<InquiryResultModel>();
            if (resultModels is null)
            {
                return result;
            }

            foreach (var inquiryResultModel in resultModels)
            {
                if (inquiryResultModel.Data is CreateInvoiceResponse createInvoiceResponse)
                {
                    result.Add(new(inquiryResultModel.ReferenceNumber,
                        inquiryResultModel.Uid,
                        inquiryResultModel.Status,
                        createInvoiceResponse,
                        inquiryResultModel.PacketType,
                        inquiryResultModel.FiscalId));
                }
                else
                {
                    result.Add(inquiryResultModel);
                }

            }
            return result;
        }
    }
}