using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Eefa.Ticketing.Application.Contract.Dtos.BasicInfos;

namespace Eefa.Ticketing.Application.ACL
{
    public class Identity : IIdentity
    {
        private readonly IConfiguration _configuration;

        public Identity(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<LoginResult> LoginAsync()
        {
            var loginUrl = _configuration.GetSection("Identity:LoginUrl").Value;
            var username = _configuration.GetSection("Identity:Username").Value;
            var password = _configuration.GetSection("Identity:Password").Value;
            if (string.IsNullOrEmpty(loginUrl) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new Exception("اطلاعات کانفیگ کامل نیست.");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(loginUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromSeconds(180000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                LoginInput loginInput = new(username, password);

                HttpResponseMessage response = await client.PostAsJsonAsync("", loginInput);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("سیستم لاگین در دسترس نیست.");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    LoginResult dto = JsonConvert.DeserializeObject<LoginResult>(content);
                    return dto;
                }
            }
        }
    }
}
