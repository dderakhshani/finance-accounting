using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Inventory.Domain;
using Newtonsoft.Json;

namespace Eefa.Invertory.Infrastructure.Services
{
    public class SinaService : ISinaService
    {
        public async Task<ICollection<SinaProduct>> GetProductList(int LastId,string url)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler);

            string parameter = $"lastid={LastId}";
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            var response = await client.GetAsync(url + parameter, CancellationToken.None);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            var requestList = JsonConvert.DeserializeObject<ICollection<SinaProduct>>(await response.Content.ReadAsStringAsync(CancellationToken.None));

            return requestList;
        }
        public async Task<ICollection<SinaProducingInputProduct>> GetInputProductToWarehouse(string Date,int firingType, string url)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler);

            string parameter = $"date={Date}&shift=-1&firingType={firingType}";
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            var response = await client.GetAsync(url + parameter, CancellationToken.None);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            var requestList = JsonConvert.DeserializeObject<ICollection<SinaProducingInputProduct>>(await response.Content.ReadAsStringAsync(CancellationToken.None));

            return requestList;
        }
        public async Task<ICollection<SinaProducingProduct>> GetOutProductToWarehouse(string Date, string url)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler);

            string parameter = $"DocumentDate={Date}";
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            var response = await client.GetAsync(url + parameter, CancellationToken.None);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            var requestList = JsonConvert.DeserializeObject<ICollection<SinaProducingProduct>>(await response.Content.ReadAsStringAsync(CancellationToken.None));

            return requestList;
        }
        public async Task<ICollection<FilesByPaymentNumber>> GetFilesByPaymentNumber(string url,string financialOperationNumber)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler);

            string parameter = $"paymentNumber={financialOperationNumber}";
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            var response = await client.GetAsync(url + parameter, CancellationToken.None);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            var requestList = JsonConvert.DeserializeObject<ICollection<FilesByPaymentNumber>>(await response.Content.ReadAsStringAsync(CancellationToken.None));

            return requestList;
        }

    }
}