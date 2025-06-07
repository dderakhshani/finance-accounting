using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.CodeRowDescription.Command.Create;
using Eefa.Accounting.Application.UseCases.CodeRowDescription.Command.Delete;
using Eefa.Accounting.Application.UseCases.CodeRowDescription.Command.Update;
using Eefa.Accounting.Application.UseCases.CodeRowDescription.Query.Get;
using Eefa.Accounting.Application.UseCases.CodeRowDescription.Query.GetAll;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class CodeRowDescriptionController : AccountingBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "CodeRowDescription-*,CodeRowDescription-Get")]
        public async Task<IActionResult> Get([FromQuery] GetCodeRowDescriptionQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "CodeRowDescription-*,CodeRowDescription-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllCodeRowDescriptionQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "CodeRowDescription-*,CodeRowDescription-Add")]
        public async Task<IActionResult> Add([FromBody] CreateCodeRowDescriptionCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "CodeRowDescription-*,CodeRowDescription-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCodeRowDescriptionCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "CodeRowDescription-*,CodeRowDescription-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteCodeRowDescriptionCommand{Id = id}));


    }
}
