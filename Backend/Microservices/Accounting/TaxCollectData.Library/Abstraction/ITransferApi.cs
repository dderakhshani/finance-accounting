using TaxCollectData.Library.Dto;
using TaxCollectData.Library.Dto.Transfer;
using TaxCollectData.Library.Enums;

namespace TaxCollectData.Library.Abstraction;

public interface ITransferApi
{
    Task<HttpResponse<AsyncResponseModel?>?> SendPacketsAsync<TRequest>(List<PacketDto<TRequest>> packets,
        string url,
        Dictionary<string, string> headers,
        bool encrypt,
        bool sign);

    Task<HttpResponse<SyncResponseModel<TResponse>?>> SendPacketAsync<TRequest, TResponse>(PacketDto<TRequest> packet,
        string url,
        Dictionary<string, string> headers,
        bool encrypt,
        bool sign);
}