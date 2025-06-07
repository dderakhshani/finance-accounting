using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Role.Command.Create;
using Eefa.Admin.Application.CommandQueries.Role.Command.Delete;
using Eefa.Admin.Application.CommandQueries.Role.Command.Update;
using Eefa.Admin.Application.CommandQueries.Role.Query.Get;
using Eefa.Admin.Application.CommandQueries.Role.Query.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class RoleController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "Roles-*,Roles-Get")]
        public async Task<IActionResult> Get([FromQuery] GetRoleQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Roles-*,Roles-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllRoleQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Roles-*,Roles-Add")]
        public async Task<IActionResult> Add([FromBody] CreateRoleCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Roles-*,Roles-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Roles-*,Roles-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteRoleCommand { Id = id }));
    }
}
