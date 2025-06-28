using Eefa.Warehouse.Application.Commands;
using Eefa.Warehouse.Application.Commands.Warehouse.Update;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Warehouse.WebApi.Controllers
{
    public class WarehouseLayoutController : WarehouseBaseControlle
    {
       
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateWarehouseLayoutCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateWarehouseLayoutCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) => Ok(await Mediator.Send(new DeleteWarehouseLayoutCommand() { Id = id }));

    }
}
