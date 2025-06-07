using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Eefa.Sale.Application.Common.Interfaces;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Eefa.Sale.Infrastructure.CrmServices
{
    public class CrmServices : ICrmServices
    {
        private readonly ICustomerRepository _repository;
        private readonly HttpClient _client;

        public CrmServices(ICustomerRepository repository)
        {
            _repository = repository;
            _client = new HttpClient();
        }
        public async Task<bool> SendCustomer(int customerId)
        {
            var customer = await _repository
                .GetAll()
                .Where(x => x.Id == customerId)
                .Include(x => x.Person)
                .ThenInclude(x => x.AccountReference)
                .Include(x => x.Person)
                .ThenInclude(x => x.PersonPhones)
                .FirstOrDefaultAsync();

            var payload = new CustomerDto
            {
                FullName = customer.Person.FirstName + " " + customer.Person.LastName,
                AccountCode = customer.Person.AccountReference.Code,
                Email = customer.Person.Email,
                Status = customer.IsActive == true ? 1 : 2,
                Mobiles = customer.Person.PersonPhones.FirstOrDefault(x => x.IsDefault == true)?.PhoneNumber,
                Username = customer.Person.PersonPhones.FirstOrDefault(x => x.IsDefault == true)?.PhoneNumber,
                Password = customer.Person.NationalNumber
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload, Formatting.None), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(CrmServiceUrls.CrmServiceBaseUrl + CrmServiceUrls.CustomerApiUrl, content);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
                throw new ApplicationException("failed to save customer in CRM");

            var response = JsonConvert.DeserializeObject<CustomerDto>(await httpResponse.Content.ReadAsStringAsync());
            
            return true;
        }
    }
}
