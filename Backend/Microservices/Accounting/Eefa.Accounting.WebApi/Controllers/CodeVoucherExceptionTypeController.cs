using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Command.Create;
using Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Command.Delete;
using Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Command.Update;
using Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Query.Get;
using Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Query.GetAll;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class CodeVoucherExtendTypeController : AccountingBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "CodeVoucherExtendType-*,CodeVoucherExtendType-Get")]
        public async Task<IActionResult> Get([FromQuery] GetCodeVoucherExtendTypeQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "CodeVoucherExtendType-*,CodeVoucherExtendType-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllCodeVoucherExtendTypeQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "CodeVoucherExtendType-*,CodeVoucherExtendType-Add")]
        public async Task<IActionResult> Add([FromBody] CreateCodeVoucherExtendTypeCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "CodeVoucherExtendType-*,CodeVoucherExtendType-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCodeVoucherExtendTypeCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "CodeVoucherExtendType-*,CodeVoucherExtendType-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteCodeVoucherExtendTypeCommand{Id = id}));


    }
}
