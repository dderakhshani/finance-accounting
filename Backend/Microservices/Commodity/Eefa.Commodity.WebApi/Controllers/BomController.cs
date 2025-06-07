using System;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Commands.Bom.Create;
using Eefa.Commodity.Application.Commands.Bom.Delete;
using Eefa.Commodity.Application.Commands.Bom.Update;
using Eefa.Commodity.Application.Queries.Bom;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Commodity.WebApi.Controllers
{
    public class BomController : ApiControllerBase
    {
        IBomQueries _bomQueries;

        public BomController(IBomQueries bomQueries)
        {
            _bomQueries = bomQueries ?? throw new ArgumentNullException(nameof(bomQueries));
        }


        [HttpGet]
        public async Task<IActionResult> Get(int id) => Ok(await _bomQueries.GetBomById(id));

        [HttpPost]
        public async Task<IActionResult> GetAll(int commodityCategoryId, PaginatedQueryModel paginatedQuery) => Ok(await _bomQueries.GetBoms(paginatedQuery));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBomCommand model) => Ok(await Mediator.Send(model));


        
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            return Ok(await Mediator.Send(new DeleteBomCommand() { Id = id }));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBomCommand model) => Ok(await Mediator.Send(model));

    }
}