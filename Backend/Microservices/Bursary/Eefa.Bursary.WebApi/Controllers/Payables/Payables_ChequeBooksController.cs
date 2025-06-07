using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Add;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Delete;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Update;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers
{
    public class Payables_ChequeBooksController : ApiControllerBase
    {
        public Payables_ChequeBooksController()
        {
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateChequeBookCommand command) => Ok(await Mediator.Send(command));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateChequeBookCommand command) => Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetChequeBookQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllChequeBookQuery query) => Ok(await Mediator.Send(query));
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteChequeBookCommand cmd) => Ok(await Mediator.Send(cmd));
    }
}
