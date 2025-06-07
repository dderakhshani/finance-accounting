using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Branch.Command.Create;
using Eefa.Admin.Application.CommandQueries.Branch.Command.Delete;
using Eefa.Admin.Application.CommandQueries.Branch.Command.Update;
using Eefa.Admin.Application.CommandQueries.Branch.Query.Get;
using Eefa.Admin.Application.CommandQueries.Branch.Query.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class BranchController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "Branches-*,Branches-Get")]
        public async Task<IActionResult> Get([FromQuery] GetBranchQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Branches-*,Branches-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllBranchQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Branches-*,Branches-Add")]
        public async Task<IActionResult> Add([FromBody] CreateBranchCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Branches-*,Branches-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateBranchCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Branches-*,Branches-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteBranchCommand{Id = id }));

    }
}
