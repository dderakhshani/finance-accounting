using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Eefa.Purchase.Domain.Entities;
using Eefa.Purchase.Infrastructure.Context;

namespace Eefa.Purchase.Infrastructure.Services.AdminApi
{
    public class AdminApiService : IAdminApiService
    {
        private readonly PurchaseContext _contex;
        public AdminApiService(PurchaseContext contex)
        {
            _contex = contex;
        }

        
        public async Task<Person> CallApiSavePesron(AdminApiService.PostPerson person,string Token, string Url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);
            HttpResponseMessage response = await client.PostAsJsonAsync(Url, person);
            response.EnsureSuccessStatusCode();
            var _person= response.Content.ReadFromJsonAsync<Person>();
            return await _person;
        }

       

        public class PostPerson
        {

            public string firstName { get; set; }
            public string lastName { get; set; }
            public string fatherName { get; set; }
            public string nationalNumber { get; set; }
            public string identityNumber { get; set; }
            public DateTime birthDate { get; set; }
            public int birthPlaceCountryDivisionId { get; set; }
            public int genderBaseId { get; set; }
            public bool taxIncluded { get; set; }
            public int legalBaseId { get; set; }
            public int governmentalBaseId { get; set; }
            public int accountReferenceGroupId { get; set; }
        }


        public class ResultPerson
        {
            public int id { get; set; }
            public string fullName { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string fatherName { get; set; }
            public string nationalNumber { get; set; }
            public string identityNumber { get; set; }
            public object insuranceNumber { get; set; }
            public object mobileJson { get; set; }
            public object email { get; set; }
            public int accountReferenceId { get; set; }
            public string accountReferenceCode { get; set; }
            public DateTime birthDate { get; set; }
            public int birthPlaceCountryDivisionId { get; set; }
            public int genderBaseId { get; set; }
            public int legalBaseId { get; set; }
            public int governmentalBaseId { get; set; }
            public object legalBaseTitle { get; set; }
            public object governmentalBaseTitle { get; set; }
            public object genderBaseTitle { get; set; }
            public int accountReferenceGroupId { get; set; }
            public object[] personAddressList { get; set; }
            public object personFingerprintsList { get; set; }
            public object phoneNumbers { get; set; }
            public object photoURL { get; set; }
            public object signatureURL { get; set; }
            public bool taxIncluded { get; set; }
        }
    }
}
