using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Unit.Command.Create;
using Eefa.Admin.Application.CommandQueries.Unit.Command.Delete;
using Eefa.Admin.Application.CommandQueries.Unit.Command.Update;
using Eefa.Admin.Application.CommandQueries.Unit.Query.Get;
using Eefa.Admin.Application.CommandQueries.Unit.Query.GetAll;
using Eefa.Admin.Application.CommandQueries.Unit.Query.GetByBranchId;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class UnitController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "Units-*,Units-Get")]
        public async Task<IActionResult> Get([FromQuery] GetUnitQuery model) => Ok(await Mediator.Send(model));

        [HttpGet]
        //[Authorize(Roles = "Units-*,Units-GetByBranchId")]
        public async Task<IActionResult> GetByBranchId([FromQuery] GetByBranchIdQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Units-*,Units-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllUnitQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Units-*,Units-Add")]
        public async Task<IActionResult> Add([FromBody] CreateUnitCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Units-*,Units-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateUnitCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Units-*,Units-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteUnitCommand{Id = id}));


    }
}
