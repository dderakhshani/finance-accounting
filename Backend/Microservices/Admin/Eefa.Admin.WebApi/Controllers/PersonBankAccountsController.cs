using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.PersonAddress.Query.GetAll;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Commands.Create;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Commands.Delete;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Commands.Update;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Queries.Get;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class PersonBankAccountsController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "PersonBankAccounts-*,PersonBankAccounts-Get")]
        public async Task<IActionResult> Get([FromQuery] GetPersonBankAccountQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "PersonBankAccounts-*,PersonBankAccounts-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllPersonBankAccountsQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "PersonBankAccounts-*,PersonBankAccounts-Add")]
        public async Task<IActionResult> Add([FromBody] CreatePersonBankAccountCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "PersonBankAccounts-*,PersonBankAccounts-Update")]
        public async Task<IActionResult> Update([FromBody] UpdatePersonBankAccountCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "PersonBankAccounts-*,PersonBankAccounts-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeletePersonBankAccountCommand { Id = id }));

    }
}
