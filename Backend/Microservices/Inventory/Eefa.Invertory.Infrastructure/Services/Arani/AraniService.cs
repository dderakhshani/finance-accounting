using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Inventory.Domain;
using Newtonsoft.Json;

namespace Eefa.Invertory.Infrastructure.Services.Arani
{
    public class AraniService : IAraniService
    {
        public async Task<ICollection<AraniPurchaseRequestModel>> GetRequestById(int requestId, string url)
        {

            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            var response = await client.GetAsync(url + $"={requestId}", CancellationToken.None);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            var requestList = JsonConvert.DeserializeObject<ICollection<AraniPurchaseRequestModel>>(await response.Content.ReadAsStringAsync(CancellationToken.None));

            return requestList;
        }
        public async Task<AraniRequestCommodityWarehouseModel> GetRequestCommodityWarehouse(string requestId, string url)
        {
            

            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            var response = await client.GetAsync(url + $"={requestId}", CancellationToken.None);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            var requestList = JsonConvert.DeserializeObject<AraniRequestCommodityWarehouseModel>(data);

            
            return requestList;

           
        }


        public async Task<AraniReturnCommodityWarehouseModel> GetReturnCommodityWarehouse(int requestId, string url)
        {
           

            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            var response = await client.GetAsync(url + $"={requestId}", CancellationToken.None);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            var requestList = JsonConvert.DeserializeObject<AraniReturnCommodityWarehouseModel>(data);

            
            return requestList;


        }

      
    }
}