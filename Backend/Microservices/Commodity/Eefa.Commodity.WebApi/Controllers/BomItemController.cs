using System;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Commands.BomItem.Create;
using Eefa.Commodity.Application.Commands.BomItem.Delete;
using Eefa.Commodity.Application.Commands.BomItem.Update;
using Eefa.Commodity.Application.Queries.Bom;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Commodity.WebApi.Controllers
{
    public class BomItemController : ApiControllerBase
    {
        IBomQueries _bomQueries;
        

        public BomItemController(IBomQueries bomQueries)
        {
            _bomQueries = bomQueries ?? throw new ArgumentNullException(nameof(bomQueries));
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(int bomId, PaginatedQueryModel paginatedQuery) => Ok(await _bomQueries.GetBomItemsByBomId(bomId, paginatedQuery));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBomItemCommand model) => Ok(await Mediator.Send(model));


        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteBomItemCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete2([FromQuery] int model) => Ok(await Mediator.Send(new DeleteBomItemCommand() { Id = model }));


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBomItemCommand model) => Ok(await Mediator.Send(model));

    }
}