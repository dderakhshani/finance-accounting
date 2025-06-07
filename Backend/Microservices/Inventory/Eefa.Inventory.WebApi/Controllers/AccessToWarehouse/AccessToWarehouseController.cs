using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class AccessToWarehouseController : ApiControllerBase
    {
        IAccessToWarehouseQueries _AccessToWarehouseQueries;
        public AccessToWarehouseController(IAccessToWarehouseQueries AccessToWarehouseQueries)
        {
            _AccessToWarehouseQueries = AccessToWarehouseQueries ?? throw new ArgumentNullException(nameof(AccessToWarehouseQueries));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserId(int UserId, string TableName)
        {
            var result = await _AccessToWarehouseQueries.GetUserId(UserId, TableName);
            return Ok(ServiceResult<List<int>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> update([FromBody] UpdateAccessToWarehouseCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAccessToWarehouseCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> delete(int UserId,int WarehouseId, string TableName)
        {
            return Ok(await Mediator.Send(new DeleteAccessToWarehouseCommand() { UserId = UserId, WarehouseId= WarehouseId, TableName = TableName }));
        }
        [HttpPost]
        public async Task<IActionResult> GetUsers(PaginatedQueryModel paginatedQuery)
        {
            var result = await _AccessToWarehouseQueries.GetUsers(paginatedQuery);

            return Ok(ServiceResult<PagedList<UserModel>>.Success(result));
        }
    }
}