using Eefa.Common.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Eefa.Sale.Application.Queries.Customers;
using Eefa.Common.Data.Query;
using Eefa.Sale.Application.Commands.Customer.Create;
using Eefa.Sale.Application.Commands.Customer.Delete;
using Eefa.Sale.Application.Commands.Customer.Update;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;
using Microsoft.EntityFrameworkCore;
using Eefa.Common.Data;
using Sale.Application.Common.Extensions;
using Eefa.Sale.Application.Common.ImportModels;

namespace Eefa.Sale.WebApi.Controllers
{
    public class CustomersController : ApiControllerBase
    {
        readonly ICustomerQueries _customerQueries;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRepository<Person> _personRepository;

        public CustomersController(ICustomerQueries customerQueries, ICustomerRepository customerRepository, IRepository<Person> personRepository)
        {
            this._customerQueries = customerQueries;
            _customerRepository = customerRepository;
            _personRepository = personRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery) => Ok(await _customerQueries.GetAll(paginatedQuery));

        [HttpGet]
        public async Task<IActionResult> Get(int id) => Ok(await _customerQueries.GetById(id));
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCustomerCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteCustomerCommand { Id = id }));

        [HttpPost]
        public async Task<IActionResult> ImportCustomersPhoneNumbers([FromForm] ImportPayload payload)
        {
            var lines = await payload.File.ReadLines();
            var headerLine = lines[0];
            var customerLines = lines.Except(new List<string>() { headerLine });


            foreach (var line in customerLines)
            {
                try
                {
                    var props = line.Split(',');
                    var customerCode = props[2];
                    var customerPhone = props[3].ToEnglishNumbers();

                    var person = await this._personRepository.GetAll().Where(x => x.IdentityNumber == customerCode).Include(x => x.PersonPhones).FirstOrDefaultAsync();
                    var isPhoneNumberAlreadyAdded = person?.PersonPhones?.Any(x => x.PhoneNumber == customerPhone) ?? false;
                    if (person != null && !isPhoneNumberAlreadyAdded)
                    {
                        person.PersonPhones.Add(new PersonPhones
                        {
                            PhoneNumber = customerPhone,
                            IsDefault = !person.PersonPhones.Any(x => x.IsDefault),
                            PhoneTypeBaseId = 28345

                        });
                        await this._personRepository.SaveChangesAsync();
                    }
                }
                catch
                {

                }
            }
            return Ok();
        }
    }
}
