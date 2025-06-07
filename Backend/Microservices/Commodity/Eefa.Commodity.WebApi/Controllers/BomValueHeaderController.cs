using System;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Commands.BomValueHeader.Create;
using Eefa.Commodity.Application.Commands.BomValueHeader.Delete;
using Eefa.Commodity.Application.Commands.BomValueHeader.Update;
using Eefa.Commodity.Application.Queries.Bom;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Commodity.WebApi.Controllers
{
    public class BomValueHeaderController : ApiControllerBase
    {
        IBomQueries _bomQueries;

        public BomValueHeaderController(IBomQueries bomQueries)
        {
            _bomQueries = bomQueries ?? throw new ArgumentNullException(nameof(bomQueries));
        }


        [HttpGet]
        public async Task<IActionResult> Get(int id) => Ok(await _bomQueries.GetBomValueHeaderById(id));

        //[HttpPost]
        //public async Task<IActionResult> GetAllByBomId(int bomId, PaginatedQueryModel paginatedQuery) => Ok(await _bomQueries.GetBomValueHeadersByBomId(bomId, paginatedQuery));
        [HttpPost]
        public async Task<IActionResult> GetAllByCommodityId(int commodityId, PaginatedQueryModel paginatedQuery) {

            var result= await _bomQueries.GetBomValueHeadersByCommodityId(commodityId, paginatedQuery);
            return Ok(result);

        }
        [HttpPost]
        public async Task<IActionResult> GetBomsByCommodityCategoryId(PaginatedQueryModel paginatedQuery)
        {

            var result = await _bomQueries.GetBomsByCommodityCategoryId(paginatedQuery);
            return Ok(result);

        }
        [HttpPost]
        public async Task<IActionResult> GetAllBomValueHeaders(PaginatedQueryModel paginatedQuery)
        {

            var result = await _bomQueries.GetAllBomValueHeaders(paginatedQuery);
            return Ok(result);

        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBomValueHeaderCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            return Ok(await Mediator.Send(new DeleteBomValueHeaderCommand() { Id = id }));
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBomValueHeaderCommand model) => Ok(await Mediator.Send(model));

    }
}