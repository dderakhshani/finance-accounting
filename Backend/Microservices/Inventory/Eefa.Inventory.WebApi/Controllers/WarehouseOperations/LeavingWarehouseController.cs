using System.Threading.Tasks;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class LeavingWarehouseController : ApiControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> LeavingPartWarehouse([FromBody] LeavingPartWarehouseCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> LeavingMaterialWarehouse([FromBody] AddLeavingMaterialWarehouseCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> UpdateLeavingMaterialWarehouse([FromBody] UpdateLeavingMaterialWarehouseCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> LeavingCosumableWarehouse([FromBody] LeavingCommodityWarehouseCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> LeaveProductWarehouse([FromBody] LeaveProductWarehouseCommand model) => Ok(await Mediator.Send(model));


        [HttpPost]
        public async Task<IActionResult> InsertLeavingWarehouseMaterial([FromBody] InsertLeavingWarehouseMaterialCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateLeavingWarehouseMaterial([FromBody] UpdateLeavingWarehouseMaterialCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> LeaveCommodity([FromBody] LeaveCommodityCommand model) => Ok(await Mediator.Send(model));


        

    }
}


