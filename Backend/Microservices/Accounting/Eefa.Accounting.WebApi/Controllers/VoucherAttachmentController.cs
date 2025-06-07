using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.VoucherAttachment.Command.Create;
using Eefa.Accounting.Application.UseCases.VoucherAttachment.Command.Delete;
using Eefa.Accounting.Application.UseCases.VoucherAttachment.Query.Get;
using Eefa.Accounting.Application.UseCases.VoucherAttachment.Query.GetAllByVoucherHeadId;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class VoucherAttachmentController : AccountingBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "VoucherAttachments-*,VoucherAttachments-Get")]
        public async Task<IActionResult> Get([FromQuery] GetVoucherAttachmentQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VoucherAttachments-*,VoucherAttachments-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllVoucherAttachmentByVoucherHeadIdQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VoucherAttachments-*,VoucherAttachments-Add")]
        public async Task<IActionResult> Add([FromBody] CreateVoucherAttachmentCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "VoucherAttachments-*,VoucherAttachments-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteVoucherAttachmentCommand { Id = id }));


    }
}
