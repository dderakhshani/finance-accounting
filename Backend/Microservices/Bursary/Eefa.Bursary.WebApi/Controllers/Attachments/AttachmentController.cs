using Eefa.Bursary.Application.UseCases.Attachments.Commands.Add;
using Eefa.Bursary.Application.UseCases.Attachments.Commands.Delete;
using Eefa.Bursary.Application.UseCases.Attachments.Commands.Update;
using Eefa.Bursary.Application.UseCases.Attachments.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Attachments
{
    public class AttachmentController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAttachmentCommand command) => Ok(await Mediator.Send(command));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAttachmentCommand command) => Ok(await Mediator.Send(command));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteAttachmentCommand command) => Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAttachmentQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllAttachmentsQuery query) => Ok(await Mediator.Send(query));

    }
}
