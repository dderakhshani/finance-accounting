using Eefa.Warehouse.Application.Commands;
using Eefa.Warehouse.Application.Commands.Warehouse.Update;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Warehouse.WebApi.Controllers
{
    public class WarehouseController : WarehouseBaseControlle
    {

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateWarehousCommand model)=>Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateWarehousCommand model) => Ok(await Mediator.Send(model));

    }
}
