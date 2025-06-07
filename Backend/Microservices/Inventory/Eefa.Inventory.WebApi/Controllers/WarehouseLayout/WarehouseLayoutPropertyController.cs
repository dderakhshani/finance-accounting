using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Inventory.Application.Models.CommodityCategory;
using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers.WarehouseLayout
{
    public class WarehouseLayoutPropertyController : ApiControllerBase
    {
        ICommodityCategoryQueries _Repository;


        public WarehouseLayoutPropertyController(

           ICommodityCategoryQueries warehouseLayoutQueries
           )
        {
            _Repository = warehouseLayoutQueries ?? throw new ArgumentNullException(nameof(warehouseLayoutQueries));
        }
        [HttpPost]
        public async Task<IActionResult> GetPropertyByWarehouseLayoutId(int warehouseLayoutId, PaginatedQueryModel paginatedQuery)
        {

            var result = await _Repository.GetPropertyByWarehouseLayoutId(warehouseLayoutId, paginatedQuery);
            return Ok(ServiceResult<PagedList<CommodityCategoryPropertyModel>>.Success(result));

        }
        [HttpDelete]

        public async Task<IActionResult> Delete(int categoryPropertyId, int warehouseLayoutPropertiesId)
        {
            return Ok(await Mediator.Send(new DeleteWarehouseLayoutPropertyCommand() { categoryPropertyId = categoryPropertyId, warehouseLayoutPropertiesId = warehouseLayoutPropertiesId }));
        }

    }
}