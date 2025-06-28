using Eefa.Common.Data.Query;
using Eefa.Common.Data;
using Eefa.Common;
using Eefa.Warehouse.Application.Commands;
using Eefa.Warehouse.Application.Commands.Warehouse.Delete;
using Eefa.Warehouse.Application.Commands.Warehouse.Update;
using Eefa.Warehouse.Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eefa.Warehouse.Application.Models;

namespace Eefa.Warehouse.WebApi.Controllers
{
    public class WarehouseController : WarehouseBaseControlle
    {
        private readonly IWarehousQueries _warehousQueries;
        public WarehouseController(IWarehousQueries warehousQueries)
        {
            _warehousQueries = warehousQueries ?? throw new ArgumentNullException(nameof(warehousQueries));
        }



        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateWarehouseCommand model)=>Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateWarehouseCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)=> Ok(await Mediator.Send(new DeleteWarehouseCommand() { Id = id }));

        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery)
        {
            var result = await _warehousQueries.GetAll(paginatedQuery);
            return Ok(ServiceResult<PagedList<WarehouseModel>>.Success(result));
        }

    }
}
