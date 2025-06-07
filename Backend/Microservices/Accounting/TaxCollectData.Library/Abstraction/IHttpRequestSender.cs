using TaxCollectData.Library.Dto;

namespace TaxCollectData.Library.Abstraction
{
    public interface IHttpRequestSender
    {
        Task<HttpResponse<T?>> SendPostRequestAsync<T>(string url, string requestBody, Dictionary<string, string> headers);
    }
}
