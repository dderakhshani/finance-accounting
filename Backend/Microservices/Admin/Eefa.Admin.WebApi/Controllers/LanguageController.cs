using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Language.Command.Create;
using Eefa.Admin.Application.CommandQueries.Language.Command.Delete;
using Eefa.Admin.Application.CommandQueries.Language.Command.Update;
using Eefa.Admin.Application.CommandQueries.Language.Query.Get;
using Eefa.Admin.Application.CommandQueries.Language.Query.GetAll;

namespace Eefa.Admin.WebApi.Controllers
{
    public class LanguageController : AdminBaseController
    {
        [HttpPost]
        //[Authorize(Roles = "Languages-*,Languages-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllLanguageQuery model) => Ok(await Mediator.Send(model));

        [HttpGet]
        //[Authorize(Roles = "Languages-*,Languages-Get")]
        public async Task<IActionResult> Get([FromQuery] GetLanguageQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Languages-*,Languages-Add")]
        public async Task<IActionResult> Add([FromBody] CreateLanguageCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Languages-*,Languages-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateLanguageCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Languages-*,Languages-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteLanguageCommand { Id = id }));

    }
}
