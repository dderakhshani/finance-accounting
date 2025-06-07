using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers.WarehouseStock
{
    public class WarehouseStockController : ApiControllerBase
    {
        IWarehouseStocksQueries _queries;



        public WarehouseStockController(IWarehouseStocksQueries queries)
        {
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));

        }


        [HttpGet]
        public async Task<bool> GetIsAvailableCommodity(int? CommodityId, string CommodityCode)
        {

            return await _queries.GetIsAvailableCommodity(CommodityId, CommodityCode);

        }
        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery)
        {

            var result = await _queries.GetAll(paginatedQuery);
            return Ok(ServiceResult<PagedList<WarehouseStockModel>>.Success(result));

        }
      
        
    }
    



}