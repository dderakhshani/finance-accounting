using Eefa.Bursary.Application.UseCases.AutoVouchers.PayableDocs.Commands.Add;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.AutoVouchers
{
    public class AutovouchersController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddPayableDocumentsAutovoucher([FromBody] AddPayableDocsAutovoucher command) => Ok(await Mediator.Send(command));

    }
}
