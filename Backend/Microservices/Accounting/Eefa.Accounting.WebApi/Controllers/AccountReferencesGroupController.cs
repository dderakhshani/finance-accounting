using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Command.Create;
using Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Command.Delete;
using Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Command.Update;
using Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Query.Get;
using Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Query.GetAll;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class AccountReferencesGroupController : AccountingBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "AccountReferencesGroups-*,AccountReferencesGroups-Get")]
        public async Task<IActionResult> Get([FromQuery] GetAccountReferencesGroupQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "AccountReferencesGroups-*,AccountReferencesGroups-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllAccountReferencesGroupQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "AccountReferencesGroups-*,AccountReferencesGroups-Add")]
        public async Task<IActionResult> Add([FromBody] CreateAccountReferencesGroupCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "AccountReferencesGroups-*,AccountReferencesGroups-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateAccountReferencesGroupCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "AccountReferencesGroups-*,AccountReferencesGroups-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteAccountReferencesGroupCommand{Id = id}));


    }
}
