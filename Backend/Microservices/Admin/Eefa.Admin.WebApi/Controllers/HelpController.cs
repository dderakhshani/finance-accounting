using System.Threading.Tasks;
using Eefa.Admin.Application.UseCases.Help.Command.Create;
using Eefa.Admin.Application.UseCases.Help.Command.Delete;
using Eefa.Admin.Application.UseCases.Help.Command.Update;
using Eefa.Admin.Application.UseCases.Help.Query.Get;
using Eefa.Admin.Application.UseCases.Help.Query.GetAll;
using Microsoft.AspNetCore.Mvc;


namespace Eefa.Admin.WebApi.Controllers
{
    public class HelpController : AdminBaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetHelpQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllHelpQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "BaseValues-*,BaseValue-Add")]
        public async Task<IActionResult> Add([FromBody] CreateHelpCommand model) => Ok(await Mediator.Send(model));


        [HttpPut]
        //[Authorize(Roles = "BaseValues-*,BaseValue-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateHelpCommand model) => Ok(await Mediator.Send(model));


        [HttpDelete]
        //[Authorize(Roles = "BaseValues-*,BaseValue-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteHelpCommand { Id = id }));


    }
}
