using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;

using Eefa.Inventory.Application;
using Eefa.Inventory.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class WarehouseCountFormController : ApiControllerBase
    {
        private readonly IWarehouseCountFormHeadQueries _warehouseCountFormHeadQueries;
        public WarehouseCountFormController(IWarehouseCountFormHeadQueries warehouseCountFormHeadQueries)
        {
            _warehouseCountFormHeadQueries = warehouseCountFormHeadQueries;
        }

        [HttpPost]
        public async Task<IActionResult> add([FromBody] CreateWarehouseCountFormCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery)
        {
            var result = await _warehouseCountFormHeadQueries.GetAll(paginatedQuery);
            return Ok(ServiceResult<PagedList<WarehouseCountFormHeadModel>>.Success(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetWarehouseCountFormHeadById(int id)
        {
            var result = await _warehouseCountFormHeadQueries.GetWarehouseCountFormHeadById(id);
            return Ok(ServiceResult<WarehouseCountFormHeadModel>.Success(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetWarehouseCountFormHeadByParentId(int parentId)
        {
            var result = await _warehouseCountFormHeadQueries.GetWarehouseCountFormHeadByParentId(parentId);
            return Ok(ServiceResult<List<WarehouseCountFormHeadModel>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetDetailsByHeadId(PaginatedQueryModel paginatedQuery, int warehouseCountFormHeadId)
        {
            var result = await _warehouseCountFormHeadQueries.GetDetailsByHeadId(paginatedQuery, warehouseCountFormHeadId);
            return Ok(ServiceResult<PagedList<WarehouseCountFormDetailsModel>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetConflictDetails(PaginatedQueryModel paginatedQuery, int warehouseCountFormHeadId)
        {
            var result = await _warehouseCountFormHeadQueries.GetAlldiscrepancies(paginatedQuery, warehouseCountFormHeadId);
            return Ok(ServiceResult<PagedList<WarehouseCountFormDetailsModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> SetState([FromBody] UpdateStateWarehousCountFormCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> SaveCountedQuantities([FromBody] BulkUpdateWarehouseCountQuantityCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> GetWarehouseCountReport(PaginatedQueryModel paginatedQuery, int warehouseCountFormHeadId)
        {
            var result = await _warehouseCountFormHeadQueries.GetWarehouseCountReport(paginatedQuery,warehouseCountFormHeadId);
            return Ok(ServiceResult<PagedList<WarehouseCountFormReport>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetWarehouseCommoditiesWithPrice(PaginatedQueryModel paginatedQuery, int warehouseId)
        {
            var result = await _warehouseCountFormHeadQueries.GetCommoditisWithPrice(paginatedQuery, warehouseId);
            return Ok(ServiceResult<PagedList<WarehouseCommodityWithPriceModel>>.Success(result));
        }

    }
}