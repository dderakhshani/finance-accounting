using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class WarehouseLayoutReportController : ApiControllerBase
    {
        IWarehouseLayoutQueries _warehouseLayoutQueries;
       
        

        public WarehouseLayoutReportController(
         
           IWarehouseLayoutQueries warehouseLayoutQueries

            )
        {
            _warehouseLayoutQueries = warehouseLayoutQueries ?? throw new ArgumentNullException(nameof(warehouseLayoutQueries));

        }

        [HttpPost]
        public async Task<IActionResult> GetWarhouseLayoutByCommodity(PaginatedQueryModel paginatedQuery)
        {

            var result = await _warehouseLayoutQueries.GetWarehouseLayoutCommodityId(paginatedQuery);
            return Ok(ServiceResult<PagedList<WarehouseLayoutsCommoditiesQuantityModel>>.Success(result));

        }
        [HttpPost]
        public async Task<IActionResult> GetWarhouseLayoutHistotyByCommodity(string FromDate, string ToDate, PaginatedQueryModel paginatedQuery)
        {

            var result = await _warehouseLayoutQueries.GetWarehouseLayoutHistoryCommodityId(FromDate, ToDate,paginatedQuery);
            return Ok(ServiceResult<PagedList<StocksCommoditiesModel>>.Success(result));

        }
        [HttpPost]
        public async Task<IActionResult> GetAllHistoriesDocument(string FromDate,string ToDate,PaginatedQueryModel paginatedQuery)   
        {
            var result = await _warehouseLayoutQueries.GetAllHistoriesDocument(FromDate, ToDate, paginatedQuery);

            return Ok(ServiceResult<PagedList<WarehouseHistoriesDocumentViewModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetWarehouseLayoutQuantities(int? warehouseId, int? CommodityId, PaginatedQueryModel query)
        {
            var result = await _warehouseLayoutQueries.GetWarehouseLayoutQuantities(warehouseId, CommodityId, query);

            return Ok(ServiceResult<PagedList<spGetWarehouseLayoutQuantities>>.Success(result));
        }
        /// <summary>
        /// Enter ERP Commodity
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetLastChangeWarehouseLayoutQuantities(int? hour)
        {
            var result = await _warehouseLayoutQueries.GetLastChangeWarehouseLayoutQuantities(hour);

            return Ok(ServiceResult<PagedList<WarehouseLayoutsCommoditiesViewArani>>.Success(result));
        }

    }
}