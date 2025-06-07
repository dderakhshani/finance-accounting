using System;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Commands.Commodity.Create;
using Eefa.Commodity.Application.Commands.Commodity.Delete;
using Eefa.Commodity.Application.Commands.Commodity.Update;
using Eefa.Commodity.Application.Queries.Commodity;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Commodity.WebApi.Controllers
{
    public class CommodityController : ApiControllerBase
    {
        ICommodityQueries _commodityQueries;
        public CommodityController(ICommodityQueries commodityQueries)
        {
            _commodityQueries = commodityQueries ?? throw new ArgumentNullException(nameof(commodityQueries));
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCommodityCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCommodityCommand model) => Ok(await Mediator.Send(model));

        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteCommodityCommand { Id = id }));

        [HttpGet]
        public async Task<IActionResult> Get(int id) => Ok(await _commodityQueries.GetById(id));

        [HttpPost]
        public async Task<IActionResult> GetAll(int CommodityCategoryId,PaginatedQueryModel paginatedQuery) => Ok(await _commodityQueries.GetAll(CommodityCategoryId,paginatedQuery));

        [HttpPut]
        public async Task<IActionResult> UpdateCommodityNationalId([FromBody] UpdateCommodityNationalIdCommand model) => Ok(await Mediator.Send(model));


    }

}