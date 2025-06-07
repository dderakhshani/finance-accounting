using Eefa.Bursary.Application.Queries.Bank;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Add;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Update;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Queries;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Queries;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Eefa.Bursary.WebApi.Controllers.Definitions
{
    public class BanksController : ApiControllerBase
    {
        public BanksController()
        {

        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBankCommand command) => Ok(await Mediator.Send(command));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBankCommand command) => Ok(await Mediator.Send(command));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteBankCommand command) => Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetBankQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllBanksQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetAllBankTypes([FromBody] GetBankTypesQuery query) => Ok(await Mediator.Send(query));

    }
}
