using System;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Commands.CommodityCategory.Create;
using Eefa.Commodity.Application.Commands.CommodityCategory.Delete;
using Eefa.Commodity.Application.Commands.CommodityCategory.Update;
using Eefa.Commodity.Application.Queries.Commodity;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Commodity.WebApi.Controllers
{
    public class CommodityCategoryController : ApiControllerBase
    {

        ICommodityQueries _commodityQueries;

        public CommodityCategoryController(ICommodityQueries commodityQueries)
        {
            _commodityQueries = commodityQueries ?? throw new ArgumentNullException(nameof(commodityQueries));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCommodityCategoryCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCommodityCategoryCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int model) => Ok(await Mediator.Send(new DeleteCommodityCategoryCommand() { Id = model }));


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int id) => Ok(await _commodityQueries.GetCategoryById(id));

        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery) => Ok(await _commodityQueries.GetAllCategories(paginatedQuery));

        [HttpGet]
        public async Task<IActionResult> GetCategoryParentTree([FromQuery] string levelCode) => Ok(await _commodityQueries.GetCategoryParentTree(levelCode));
    }
}