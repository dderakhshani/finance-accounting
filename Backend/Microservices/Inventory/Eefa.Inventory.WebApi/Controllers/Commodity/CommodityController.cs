using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers.CommodityCategories
{


    public class CommodityController : ApiControllerBase
    {
        ICommodityQueries _Repository;
        public CommodityController(

           ICommodityQueries repository

            )
        {
            _Repository = repository ?? throw new ArgumentNullException(nameof(repository));

        }

        [HttpPost]
        public async Task<IActionResult> GetCommodity(int? warehouseId, string searchTerm, bool? isOnlyFilterByWarehouse, PaginatedQueryModel paginatedQuery)
        {
           var result = await _Repository.GetCommodity(warehouseId, isOnlyFilterByWarehouse, searchTerm, paginatedQuery);
            return Ok(ServiceResult<PagedList<ViewCommodityModel>>.Success(result));

        }
       
        [HttpPost]
        public async Task<IActionResult> GetCommodityById(int Id, string searchTerm, PaginatedQueryModel paginatedQuery)
        {
            var result = await _Repository.GetCommodityById(Id, paginatedQuery);
            return Ok(ServiceResult<PagedList<ViewCommodityModel>>.Success(result));

        }
        
        [HttpGet]
        public async Task<IActionResult> GetQuantityCommodity(int WarehouseId, int CommodityId)
        {

            var result =  _Repository.GetQuantityCommodity(WarehouseId,CommodityId);
            return Ok(ServiceResult<double>.Success(result));
        }

    }
}
