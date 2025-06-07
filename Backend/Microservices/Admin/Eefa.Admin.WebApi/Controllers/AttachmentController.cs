using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Attachment.Command.Delete;
using Eefa.Admin.Application.CommandQueries.Attachment.Command.Update;
using Eefa.Admin.Application.CommandQueries.Attachment.Query.Get;
using Eefa.Admin.Application.CommandQueries.Attachment.Query.GetAll;
using Eefa.Admin.Application.CommandQueries.Attachment.Query.Search;

namespace Eefa.Admin.WebApi.Controllers
{
    public class AttachmentController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "Attachments-*,Attachments-Get")]
        public async Task<IActionResult> Get([FromQuery] GetAttachmentQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Attachments-*,Attachments-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllAttachmentQuery model) => Ok(await Mediator.Send(model));


        [HttpPut]
        //[Authorize(Roles = "Attachments-*,Attachments-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateAttachmentCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Attachments-*,Attachments-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteAttachmentCommand { Id = id }));

        [HttpPost]
        //[Authorize(Roles = "Attachments-*,Attachments-Search")]
        public async Task<IActionResult> Search([FromBody] SearchAttachmentQuery model) => Ok(await Mediator.Send(model));
    }
}
