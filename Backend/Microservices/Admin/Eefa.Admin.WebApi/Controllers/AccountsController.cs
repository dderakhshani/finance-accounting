using Eefa.Admin.Application.CommandQueries.Accounts.Commands.Create;
using Eefa.Admin.Application.CommandQueries.Accounts.Commands.Delete;
using Eefa.Admin.Application.CommandQueries.Accounts.Commands.Update;
using Eefa.Admin.Application.CommandQueries.Accounts.Queries.Get;
using Eefa.Admin.Application.CommandQueries.Accounts.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Admin.WebApi.Controllers
{
    public class AccountsController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "Accountes-*,Accountes-Get")]
        public async Task<IActionResult> Get([FromQuery] GetAccountQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Accountes-*,Accountes-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllAccountsQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Accountes-*,Accountes-Add")]
        public async Task<IActionResult> Add([FromBody] CreateAccountCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Accountes-*,Accountes-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateAccountCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Accountes-*,Accountes-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteAccountCommand { Id = id }));
    }
}
