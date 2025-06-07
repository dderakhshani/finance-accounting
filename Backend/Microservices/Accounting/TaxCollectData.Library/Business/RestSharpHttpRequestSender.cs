using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Dto;

namespace TaxCollectData.Library.Business
{
    internal class RestSharpHttpRequestSender : IHttpRequestSender
    {
        private readonly HttpClient _httpClient;

        public RestSharpHttpRequestSender(HttpClient httpClient)
        {
            _httpClient = httpClient;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public async Task<HttpResponse<T?>> SendPostRequestAsync<T>(string url,
            string requestBody,
            Dictionary<string, string> headers)
        {
            using var request = GetHttpRequestMessage<T>(url, headers, requestBody);
            using var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            try
            {
                var body = await response.Content.ReadFromJsonAsync<T>(JsonSerializerConfig.JsonSerializerOptions).ConfigureAwait(false);
                return new HttpResponse<T?>(body, (int)response.StatusCode);
            }
            catch (Exception e)
            {
                return new HttpResponse<T?>(e is AuthenticationException ? 496 : 408);
            }
        }

        private static HttpRequestMessage GetHttpRequestMessage<T>(string url, Dictionary<string, string> headers, string content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (var item in headers)
            {
                request.Headers.Add(item.Key, item.Value);
            }

            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            return request;
        }
    }
}