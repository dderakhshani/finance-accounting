using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Permission.Command.Create;
using Eefa.Admin.Application.CommandQueries.Permission.Command.Delete;
using Eefa.Admin.Application.CommandQueries.Permission.Command.Update;
using Eefa.Admin.Application.CommandQueries.Permission.Query.Get;
using Eefa.Admin.Application.CommandQueries.Permission.Query.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class PermissionController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "Permissions-*,Permissions-Get")]
        public async Task<IActionResult> Get([FromQuery] GetPermissionQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Permissions-*,Permissions-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllPermissionQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Permissions-*,Permissions-Add")]
        public async Task<IActionResult> Add([FromBody] CreatePermissionCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Permissions-*,Permissions-Update")]
        public async Task<IActionResult> Update([FromBody] UpdatePermissionCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Permissions-*,Permissions-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeletePermissionCommand { Id = id }));       
       
    }
}
