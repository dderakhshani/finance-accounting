using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.MenuItem.Command.Create;
using Eefa.Admin.Application.CommandQueries.MenuItem.Command.Delete;
using Eefa.Admin.Application.CommandQueries.MenuItem.Command.Update;
using Eefa.Admin.Application.CommandQueries.MenuItem.Query.Get;
using Eefa.Admin.Application.CommandQueries.MenuItem.Query.GetAll;
using Eefa.Admin.Application.UseCases.MenuItem.Query.GetAllRoleByMenuItemPermissionId;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class MenuItemController : AdminBaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetMenuItemQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllMenuItemQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> GetAllRoleByMenuItemPermissionId([FromBody] GetAllRoleByMenuItemPermissionIdQuery model)=> Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateMenuItemCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMenuItemCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteMenuItemCommand{Id = id}));

    }
}
