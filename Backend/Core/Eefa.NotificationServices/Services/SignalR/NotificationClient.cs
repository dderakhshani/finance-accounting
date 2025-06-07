using Eefa.NotificationServices.Common.Model;
using Eefa.NotificationServices.Dto;
using Eefa.NotificationServices.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.NotificationServices.Services.SignalR
{
    public class NotificationClient:INotificationClient 
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public NotificationClient(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task Send(NotificationDto message)
        {
            var urlAddress = _configuration.GetSection("NotificationUri:Url").Value;
            var requestUri = new Uri(urlAddress);          
                  
          //  await _httpClient.PostAsJsonAsync(requestUri, message);
          
        }
    }
}
