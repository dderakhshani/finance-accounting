using System;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Commands.CommodityCategoryPropertyItem.Create;
using Eefa.Commodity.Application.Commands.CommodityCategoryPropertyItem.Delete;
using Eefa.Commodity.Application.Commands.CommodityCategoryPropertyItem.Update;
using Eefa.Commodity.Application.Queries.Commodity;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Commodity.WebApi.Controllers
{
    public class CommodityCategoryPropertyItemController : ApiControllerBase
    {
        ICommodityQueries _commodityQueries;


        public CommodityCategoryPropertyItemController(ICommodityQueries commodityQueries)
        {
            _commodityQueries = commodityQueries ?? throw new ArgumentNullException(nameof(commodityQueries));
        }


   

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCommodityCategoryPropertyItemCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCommodityCategoryPropertyItemCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int model) => Ok(await Mediator.Send(new DeleteCommodityCategoryPropertyItemCommand(){Id = model}));


        [HttpGet]
        public async Task<IActionResult> Get(int id) => Ok(await _commodityQueries.GetCategoryPropertyItemById(id));

        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery) => Ok(await _commodityQueries.GetAllCategoryPropertyItems(paginatedQuery));


    }
}