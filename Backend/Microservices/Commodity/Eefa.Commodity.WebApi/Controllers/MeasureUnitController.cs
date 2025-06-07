using System;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Commands.MeasureUnit.Create;
using Eefa.Commodity.Application.Commands.MeasureUnit.Delete;
using Eefa.Commodity.Application.Commands.MeasureUnit.Update;
using Eefa.Commodity.Application.Queries.Measure;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Commodity.WebApi.Controllers
{
    public class MeasureUnitController : ApiControllerBase
    {
        IMeasureQueries _measureQueries;

        public MeasureUnitController(IMeasureQueries measureQueries)
        {
            _measureQueries = measureQueries ?? throw new ArgumentNullException(nameof(measureQueries));
        }


        [HttpGet]
        public async Task<IActionResult> Get(int id) => Ok(await _measureQueries.GetMeasureUnitById(id));

        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery) => Ok(await _measureQueries.GetMeasures(paginatedQuery));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateMeasureUnitCommand model) => Ok(await Mediator.Send(model));


        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteMeasureUnitCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete2([FromQuery] int model) => Ok(await Mediator.Send(new DeleteMeasureUnitCommand(){Id = model}));


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMeasureUnitCommand model) => Ok(await Mediator.Send(model));

    }
}