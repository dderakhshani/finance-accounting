using System;
using System.Threading.Tasks;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class WarehouseOperationsController : ApiControllerBase
    {
        IReceiptQueries _receiptQueries;
       

        public WarehouseOperationsController(IReceiptQueries receiptQueries, IAccountReferences accountReferences)
        {
            _receiptQueries = receiptQueries ?? throw new ArgumentNullException(nameof(receiptQueries));

        }
        [HttpPost]
        public async Task<IActionResult> PlacementWarehouseDirectReceipt([FromBody] PlacementWarehouseDirectReceiptCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> PlacementWarehouse([FromBody] PlacementWarehouseCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusDirectReceiptCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> ChangePlacementWarehouseDirectReceipt([FromBody] ChangePlacementCommand model) => Ok(await Mediator.Send(model));
        
        [HttpPost]
        public async Task<IActionResult> AddWarehouseInventory([FromBody] AddWarehouseInventoryCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> EditWarehouseLayoutInventory([FromBody] EditWarehousInventoryCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateWarehouseCommodityAvailable([FromBody] UpdateWarehouseCommodityAvailableCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateWarehouseCommodityPrice([FromBody] UpdateWarehouseCommodityPriceCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateWarehouseLayoutALL([FromBody] UpdateWarehouseLayoutALLCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> RemoveCommodityFromWarehouse([FromBody] RemoveCommodityFromWarehouseCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateAddNewCommodity([FromBody] UpdateAddNewCommodityCommand model) => Ok(await Mediator.Send(model));

        

    }
}