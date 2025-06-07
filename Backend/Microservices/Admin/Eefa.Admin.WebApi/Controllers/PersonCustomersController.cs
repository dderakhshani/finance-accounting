using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.PersonCustomer.Commands.Create;
using Eefa.Admin.Application.CommandQueries.PersonCustomer.Commands.Delete;
using Eefa.Admin.Application.CommandQueries.PersonCustomer.Commands.Update;
using Eefa.Admin.Application.CommandQueries.PersonCustomer.Queries.Get;
using Eefa.Admin.Application.CommandQueries.PersonCustomer.Queries.GetAll;

using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class PersonCustomersController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "PersonCustomers-*,PersonCustomers-Get")]
        public async Task<IActionResult> Get([FromQuery] GetPersonCustomerQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "PersonCustomers-*,PersonCustomers-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllPersonCustomersQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "PersonCustomers-*,PersonCustomers-Add")]
        public async Task<IActionResult> Add([FromBody] CreatePersonCustomerCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "PersonCustomers-*,PersonCustomers-Update")]
        public async Task<IActionResult> Update([FromBody] UpdatePersonCustomerCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "PersonCustomers-*,PersonCustomers-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeletePersonCustomerCommand { Id = id }));

    }
}
