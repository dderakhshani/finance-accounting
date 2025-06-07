using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.AccountHead.Command.Create;
using Eefa.Accounting.Application.UseCases.AccountHead.Command.Delete;
using Eefa.Accounting.Application.UseCases.AccountHead.Command.Update;
using Eefa.Accounting.Application.UseCases.AccountHead.Query.Get;
using Eefa.Accounting.Application.UseCases.AccountHead.Query.GetAll;

using Library.Interfaces;


namespace Eefa.Accounting.WebApi.Controllers
{



    public class AccountHeadController : AccountingBaseController
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public AccountHeadController( ICurrentUserAccessor currentUserAccessor)
        {
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet]
        //[Authorize(Roles = "AccountHead-*,AccountHead-Get")]
        public async Task<IActionResult> Get([FromQuery] GetAccountHeadQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "AccountHead-*,AccountHead-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllAccountHeadQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "AccountHead-*,AccountHead-Add")]
        public async Task<IActionResult> Add([FromBody] CreateAccountHeadCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "AccountHead-*,AccountHead-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateAccountHeadCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "AccountHead-*,AccountHead-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteAccountHeadCommand { Id = id }));

    }
}
