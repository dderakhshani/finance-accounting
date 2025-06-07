using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Queries;
using Eefa.Bursary.Application.UseCases.Payables.Documents.Commands.Add;
using Eefa.Bursary.Application.UseCases.Payables.Documents.Commands.Delete;
using Eefa.Bursary.Application.UseCases.Payables.Documents.Commands.Update;
using Eefa.Bursary.Application.UseCases.Payables.Documents.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Payables
{
    public class Payables_DocumentsController : ApiControllerBase
    {
        public Payables_DocumentsController()
        {
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateDocumentCommand command) => Ok(await Mediator.Send(command));
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDocumentCommand command) => Ok(await Mediator.Send(command));
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteDocumentCommand command) => Ok(await Mediator.Send(command));
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetDocumentQuery query) => Ok(await Mediator.Send(query));
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllDocumentsQuery query) => Ok(await Mediator.Send(query));
        [HttpPost]
        public async Task<IActionResult> GetDocumentsList([FromBody] GetDocumentQuery_View query) => Ok(await Mediator.Send(query));
        [HttpPost]
        public async Task<IActionResult> GetDocumentAccountsList([FromBody] GetDocumentAccountQuery query) => Ok(await Mediator.Send(query));
        [HttpPost]
        public async Task<IActionResult> GetDocumentOperationsList([FromBody] GetDocumentOperationQuery query) => Ok(await Mediator.Send(query));
        [HttpPost]
        public async Task<IActionResult> GetDocumentPayOrdersList([FromBody] GetDocumentsPayOrderQuery query)=>Ok(await Mediator.Send(query));
        [HttpPost]
        public async Task<IActionResult> GetPaySubjectsList([FromBody] GetPaySubjectsQuery query) => Ok(await Mediator.Send(query));

    }
}
