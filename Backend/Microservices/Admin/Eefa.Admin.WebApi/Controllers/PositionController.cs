using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Position.Command.Create;
using Eefa.Admin.Application.CommandQueries.Position.Command.Delete;
using Eefa.Admin.Application.CommandQueries.Position.Command.Update;
using Eefa.Admin.Application.CommandQueries.Position.Query.Get;
using Eefa.Admin.Application.CommandQueries.Position.Query.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class PositionController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "Positions-*,Positions-Get")]
        public async Task<IActionResult> Get([FromQuery] GetPositionQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Positions-*,Positions-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllPositionQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Positions-*,Positions-Add")]
        public async Task<IActionResult> Add([FromBody] CreatePositionCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Positions-*,Positions-Update")]
        public async Task<IActionResult> Update([FromBody] UpdatePositionCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Positions-*,Positions-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeletePositionCommand{Id = id}));

    }
}
