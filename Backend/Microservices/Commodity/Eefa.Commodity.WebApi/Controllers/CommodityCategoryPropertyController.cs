using System;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Commands.CommodityCategoryProperty.Create;
using Eefa.Commodity.Application.Commands.CommodityCategoryProperty.Delete;
using Eefa.Commodity.Application.Commands.CommodityCategoryProperty.Update;
using Eefa.Commodity.Application.Queries.Commodity;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Commodity.WebApi.Controllers
{
    public class CommodityCategoryPropertyController : ApiControllerBase
    {
        ICommodityQueries _commodityQueries;

        public CommodityCategoryPropertyController(ICommodityQueries commodityQueries)
        {
            _commodityQueries = commodityQueries ?? throw new ArgumentNullException(nameof(commodityQueries));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCommodityCategoryPropertyCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateCommodityCategoryPropertyCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteCommodityCategoryPropertyCommand() { Id = id }));


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int id) => Ok(await _commodityQueries.GetCategoryPropertyById(id));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _commodityQueries.GetAllCategoryProperties(paginatedQuery));
    }
}