using Eefa.Ticketing.Application.Contract.Dtos.BasicInfos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace Eefa.Ticketing.Application.ACL
{
    public class Admin : IAdmin
    {
        private readonly IConfiguration _configuration;

        public Admin(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<RoleAllResult> GetAllRoleAsync(string token)
        {
            var baseurl = _configuration.GetSection("Admin:BaseUrl").Value;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl + "/Role/GetAll");
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromSeconds(180000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                RoleAllInput input = new(0, 1000);

                HttpResponseMessage response = await client.PostAsJsonAsync("", input);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("سیستم ادمین در دسترس نیست.");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    RoleAllResult dto = JsonConvert.DeserializeObject<RoleAllResult>(content);
                    return dto;
                }
            }

        }

        public async Task<GetUsersByRoleIdResult> GetUsersByRoleIdAsync(int roleId, string token)
        {
            var baseurl = _configuration.GetSection("Admin:BaseUrl").Value;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl + "/User/GetAllByRoleId");
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromSeconds(180000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                GetUsersByRoleIdInput input = new(0, 1000, roleId);

                HttpResponseMessage response = await client.GetAsync("?RoleId=" + input.RoleId + "&PageIndex=" + input.PageIndex + "&PageSize=" + input.PageSize);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("سیستم ادمین در دسترس نیست.");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    GetUsersByRoleIdResult dto = JsonConvert.DeserializeObject<GetUsersByRoleIdResult>(content);
                    return dto;
                }
            }
        }

        public async Task<GetRoleTree> GetRoleTreeAsync(string token, int roleId)
        {

            var baseurl = _configuration.GetSection("Admin:BaseUrl").Value;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl + "/Role/GetTree");
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromSeconds(180000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                HttpResponseMessage response = await client.GetAsync($"?RoleId={roleId}");
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("سیستم ادمین در دسترس نیست.");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    GetRoleTree dto = JsonConvert.DeserializeObject<GetRoleTree>(content);
                    return dto;
                }
            }
        }
        public async Task<GetRoleById> GetRoleById(int id, string token)
        {
            var baseurl = _configuration.GetSection("Admin:BaseUrl").Value;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl + "/Role/Get");
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromSeconds(180000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                HttpResponseMessage response = await client.GetAsync($"?Id={id}");
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("سیستم ادمین در دسترس نیست.");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    GetRoleById dto = JsonConvert.DeserializeObject<GetRoleById>(content);
                    return dto;
                }
            }
        }
        public async Task<GetUsersByIdsResult> GetUsersByIdsAsync(List<int> userIds, string token)
        {

            var baseurl = _configuration.GetSection("Admin:BaseUrl").Value;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl + "/User/GetUsersByIds");
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromSeconds(180000);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                GetUsersByIdsInput input = new(userIds);

                HttpResponseMessage response = await client.PostAsJsonAsync("", input);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("سیستم ادمین در دسترس نیست.");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    GetUsersByIdsResult dto = JsonConvert.DeserializeObject<GetUsersByIdsResult>(content);
                    return dto;
                }
            }
        }
    }
}
