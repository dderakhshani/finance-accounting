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


    public class CommodityCategoriesController : ApiControllerBase
    {
       
        private readonly ICommodityCategoryQueries _commodityCategoryQueries;
        
        public CommodityCategoriesController(ICommodityCategoryQueries commodityCategoryQueries)  
        {
            _commodityCategoryQueries = commodityCategoryQueries ?? throw new ArgumentNullException(nameof(commodityCategoryQueries));

        }
        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery)
        {

            var result = await _commodityCategoryQueries.GetAll(paginatedQuery);
            return Ok(ServiceResult<PagedList<CommodityCategoryModel>>.Success(result));

        }
        [HttpPost]
        public async Task<IActionResult> GetTreeAll(PaginatedQueryModel paginatedQuery)
        {

            var result = await _commodityCategoryQueries.GetTreeAll(paginatedQuery);
            return Ok(ServiceResult<PagedList<CommodityCategoryModel>>.Success(result));

        }

        [HttpPost]
        public async Task<IActionResult> GetCategoresCodeAssetGroup(PaginatedQueryModel paginatedQuery)
        {
            var result = await _commodityCategoryQueries.GetCategoresCodeAssetGroup(paginatedQuery);
            return Ok(ServiceResult<PagedList<CommodityCategoryModel>>.Success(result));


        }
        
    }
}
