using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;

using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class WarehouseController : ApiControllerBase
    {
        IWarehousQueries _warehousQueries;

        public WarehouseController(
           
           IWarehousQueries warehousQueries
          )
        {
            _warehousQueries = warehousQueries ?? throw new ArgumentNullException(nameof(warehousQueries));
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {

            var result = await _warehousQueries.GetById(id);
            return Ok(ServiceResult<WarehouseModel>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery) {

           var result= await _warehousQueries.GetAll(paginatedQuery);
           return Ok(ServiceResult<PagedList<WarehouseModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetWarehousesLastLevel(PaginatedQueryModel paginatedQuery)
        {

            var result = await _warehousQueries.GetWarehousesLastLevel(paginatedQuery);
            return Ok(ServiceResult<PagedList<WarehousesLastLevelViewModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetWarehousesLastLevelByCodeVoucherGroupId(int CodeVoucherGroupId, PaginatedQueryModel paginatedQuery)
        {

            var result = await _warehousQueries.GetWarehousesLastLevelByCodeVoucherGroupId(CodeVoucherGroupId,paginatedQuery);
            return Ok(ServiceResult<PagedList<WarehousesLastLevelViewModel>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> add([FromBody] CreateWarehouseCommand model) => Ok(await Mediator.Send(model));
        

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            
            return Ok(await Mediator.Send(new DeleteWarehouseCommand(){ Id= id }));
        }
        [HttpPut]
        public async Task<IActionResult> update([FromBody] UpdateWarehouseCommand model) => Ok(await Mediator.Send(model));

    }
}