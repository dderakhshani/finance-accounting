using System;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Commands.BomValue.Create;
using Eefa.Commodity.Application.Commands.BomValue.Delete;
using Eefa.Commodity.Application.Commands.BomValue.Update;
using Eefa.Commodity.Application.Queries.Bom;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Commodity.WebApi.Controllers
{
    public class BomValueController : ApiControllerBase
    {
        IBomQueries _bomQueries;

        public BomValueController(IBomQueries bomQueries)
        {
            _bomQueries = bomQueries ?? throw new ArgumentNullException(nameof(bomQueries));
        }


        [HttpGet]
        public async Task<IActionResult> Get(int id) => Ok(await _bomQueries.GetBomValueById(id));

        //[HttpPost]
        //public async Task<IActionResult> GetAll(int bomValueHeaderId, PaginatedQueryModel paginatedQuery) => Ok(await _bomQueries.GetBomValuesByBomValueHeaderId(bomValueHeaderId, paginatedQuery));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBomValueCommand model) => Ok(await Mediator.Send(model));


        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteBomValueCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete2([FromQuery] int model) => Ok(await Mediator.Send(new DeleteBomValueCommand(){Id = model}));


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBomValueCommand model) => Ok(await Mediator.Send(model));

    }
}