using Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Add;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Update;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Queries;
using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Commands.Add;
using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Commands.Delete;
using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Commands.Update;
using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Definitions
{
    public class BankAccountsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBankAccountCommand command) => Ok(await Mediator.Send(command));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBankAccountCommand command) => Ok(await Mediator.Send(command));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteBankAccountCommand command) => Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetBankAccountQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllBankAccountsQuery query) => Ok(await Mediator.Send(query));
        [HttpPost]
        public async Task<IActionResult> GetBankAccountTypesList([FromBody] GetBankAccountTypeQuery query) => Ok(await Mediator.Send(query));

    }
}
