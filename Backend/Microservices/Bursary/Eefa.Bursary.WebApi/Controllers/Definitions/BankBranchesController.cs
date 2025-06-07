using Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Add;
using Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Delete;
using Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Update;
using Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Definitions
{
    public class BankBranchesController : ApiControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBankBranchCommand command) => Ok(await Mediator.Send(command));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBankBranchCommand command) => Ok(await Mediator.Send(command));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteBankBranchCommand command) => Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetBankBranchQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllBankBranchesQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetCountryDivisionList([FromBody] GetCountryDivisionsListQuery query) => Ok(await Mediator.Send(query));

    }
}
