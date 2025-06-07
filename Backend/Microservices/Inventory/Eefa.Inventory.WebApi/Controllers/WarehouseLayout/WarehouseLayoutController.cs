using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;

using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers.WarehouseLayout
{
    public class WarehouseLayoutController : ApiControllerBase
    {
        IWarehouseLayoutQueries _warehouseLayoutQueries;
       
        

        public WarehouseLayoutController(
         
           IWarehouseLayoutQueries warehouseLayoutQueries

            )
        {
            _warehouseLayoutQueries = warehouseLayoutQueries ?? throw new ArgumentNullException(nameof(warehouseLayoutQueries));

        }
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {

            var result = await _warehouseLayoutQueries.GetById(id);
            return Ok(ServiceResult<WarehouseLayoutGetIdModel>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery)
        {

            var result = await _warehouseLayoutQueries.GetAll(paginatedQuery);
            return Ok(ServiceResult<PagedList<WarehouseLayoutModel>>.Success(result));

        }
        
        
        
        [HttpPost]
        public async Task<IActionResult> GetParentIdAllChildByCapacityAvailabe(int id)
        {

            var result = await _warehouseLayoutQueries.GetParentIdAllChildByCapacityAvailable(id);
            return Ok(ServiceResult<PagedList<WarehouseLayoutModel>>.Success(result));

        }
        [HttpPost]
        public async Task<IActionResult> GetTreeAll(PaginatedQueryModel paginatedQuery)
        {

            var result = await _warehouseLayoutQueries.GetTreeAll(paginatedQuery);
            return Ok(ServiceResult<PagedList<WarehouseLayoutModel>>.Success(result));

        }
        [HttpPost]
        public async Task<IActionResult> GetSuggestionWarehouseLayoutByCommodityCategories(PaginatedQueryModel paginatedQuery, int commodityId)
        {

            var result = await _warehouseLayoutQueries.GetSuggestionWarehouseLayoutByCommodityCategories(paginatedQuery,commodityId);
            return Ok(ServiceResult<PagedList<WarehouseLayoutModel>>.Success(result));

        }

        

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteWarehouseLayoutCommand() { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateWarehouseLayoutCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateWarehouseLayoutCommand model) => Ok(await Mediator.Send(model));

    }
}