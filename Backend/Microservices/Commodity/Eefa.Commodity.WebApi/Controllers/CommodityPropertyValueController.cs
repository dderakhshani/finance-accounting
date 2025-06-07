using System;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Commands.CommodityPropertyValue.Create;
using Eefa.Commodity.Application.Commands.CommodityPropertyValue.Delete;
using Eefa.Commodity.Application.Commands.CommodityPropertyValue.Update;
using Eefa.Commodity.Application.Queries.Commodity;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Commodity.WebApi.Controllers
{
    public class CommodityPropertyValueController : ApiControllerBase
    {
        ICommodityQueries _commodityQueries;
        public CommodityPropertyValueController(CommodityQueries commodityQueries)
        {
            _commodityQueries = commodityQueries ?? throw new ArgumentNullException(nameof(commodityQueries));
        }



        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCommodityPropertyValueCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCommodityPropertyValueCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int model) => Ok(await Mediator.Send(new DeleteCommodityPropertyValueCommand() { Id = model }));



        [HttpGet]
        public async Task<IActionResult> Get(int commodityId) => Ok(await _commodityQueries.GetPropertyValueById(commodityId));
        
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _commodityQueries.GetAllPropertyValues(paginatedQuery));

      
   
    }
}