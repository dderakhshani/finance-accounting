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
    public class WarehouseLayoutCategoriesController : ApiControllerBase
    {
        ICommodityCategoryQueries _Repository;
        
        public WarehouseLayoutCategoriesController(

           ICommodityCategoryQueries warehouseLayoutQueries

            )
        {
            _Repository = warehouseLayoutQueries ?? throw new ArgumentNullException(nameof(warehouseLayoutQueries));

        }


        [HttpPost]
        public async Task<IActionResult> GetCategores(int? ParentId,int warhoseId, PaginatedQueryModel paginatedQuery)
        {

            var result = await _Repository.GetCategores(ParentId, warhoseId, paginatedQuery);
            return Ok(ServiceResult<PagedList<CommodityCategoryModel>>.Success(result));

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteWarehouseLayoutCategoriesCommand() { Id = id }));
        }
    }
}