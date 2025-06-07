using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.AccountReference.Command.Delete;
using Eefa.Accounting.Application.UseCases.AccountReference.Command.RemovereferencesFromGroup;
using Eefa.Accounting.Application.UseCases.AccountReference.Command.Update;
using Eefa.Accounting.Application.UseCases.AccountReference.Query.Get;
using Eefa.Accounting.Application.UseCases.AccountReference.Query.GetAll;
using Eefa.Accounting.Application.UseCases.AccountReference.Query.GetAllByGroupId;
using Eefa.Accounting.Application.UseCases.AccountReference.Command.Create;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class AccountReferenceController : AccountingBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "AccountReferences-*,AccountReferences-Get")]
        public async Task<IActionResult> Get([FromQuery] GetAccountReferenceQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "AccountReferences-*,AccountReferences-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllAccountReferenceQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "AccountReferences-*,AccountReferences-GetAllByGroupId")]
        public async Task<IActionResult> GetAllByGroupId([FromBody] GetAllAccountReferenceByGroupIdQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAccountReferenceCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "AccountReferences-*,AccountReferences-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateAccountReferenceCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "AccountReferences-*,AccountReferences-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteAccountReferenceCommand{ Id = id}));   
        
        [HttpDelete]
        //[Authorize(Roles = "AccountReferences-*,AccountReferences-RemoveFromGroup")]
        public async Task<IActionResult> RemoveFromGroup([FromBody] RemoveReferencesFromGroupCommand model) => Ok(await Mediator.Send(model));

    }
}