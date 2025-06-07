using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Command.Create;
using Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Command.Delete;
using Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Command.Update;
using Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Query.Get;
using Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Query.GetAll;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class CodeVoucherGroupController : AccountingBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "CodeVoucherGroups-*,CodeVoucherGroups-Get")]
        public async Task<IActionResult> Get([FromQuery] GetCodeVoucherGroupQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "CodeVoucherGroups-*,CodeVoucherGroups-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllCodeVoucherGroupQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "CodeVoucherGroups-*,CodeVoucherGroups-Add")]
        public async Task<IActionResult> Add([FromBody] CreateCodeVoucherGroupCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "CodeVoucherGroups-*,CodeVoucherGroups-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCodeVoucherGroupCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "CodeVoucherGroups-*,CodeVoucherGroups-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteCodeVoucherGroupCommand{Id = id}));

    }
}
