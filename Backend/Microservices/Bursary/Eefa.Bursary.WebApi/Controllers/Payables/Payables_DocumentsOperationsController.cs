using Eefa.Bursary.Application.UseCases.Payables.DocumentOperations.Commands.Add;
using Eefa.Bursary.Application.UseCases.Payables.DocumentOperations.Commands.Delete;
using Eefa.Bursary.Application.UseCases.Payables.DocumentOperations.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Payables
{
    public class Payables_DocumentsOperationsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateDocumentOperationCommand command) => Ok(await Mediator.Send(command));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteDocumentOperationCommand command) => Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetDocumentOperationQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllDocumentOperationQuery query) => Ok(await Mediator.Send(query));

    }
}
