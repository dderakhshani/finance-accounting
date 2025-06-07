using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.Cancel;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.Delete;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.UnCancel;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.Update;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Payables
{
    public class Payables_ChequeBookSheetsController : ApiControllerBase
    {
        public Payables_ChequeBookSheetsController()
        {
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateChequeBookSheetCommand cmd) => Ok(await Mediator.Send(cmd));
        [HttpPut]
        public async Task<IActionResult> CancelSheet([FromBody] CancelChequeBookSheetCommand cmd) => Ok(await Mediator.Send(cmd));
        [HttpPut]
        public async Task<IActionResult> UnCancelSheet([FromBody] UnCancelChequeBookSheetCommand cmd) => Ok(await Mediator.Send(cmd));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteChequeBookSheetCommand cmd) => Ok(await Mediator.Send(cmd));
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetChequeBookSheetQuery qry) => Ok(await Mediator.Send(qry));
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllChequeBookSheetsQuery qry) => Ok(await Mediator.Send(qry));

    }
}
