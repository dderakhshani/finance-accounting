using System;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Commands.MeasureUnitConversion.Create;
using Eefa.Commodity.Application.Commands.MeasureUnitConversion.Delete;
using Eefa.Commodity.Application.Commands.MeasureUnitConversion.Update;
using Eefa.Commodity.Application.Queries.Measure;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Commodity.WebApi.Controllers
{
    public class MeasureUnitConversionController : ApiControllerBase
    {
        IMeasureQueries _measureQueries;

        public MeasureUnitConversionController(IMeasureQueries measureQueries)
        {
            _measureQueries = measureQueries ?? throw new ArgumentNullException(nameof(measureQueries));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await _measureQueries.GetConversionById(id));

        [HttpPost]
        public async Task<IActionResult> GetAllByMeasureUnitId(int measureUnitId, PaginatedQueryModel paginatedQuery) => Ok(await _measureQueries.GetConversionsByMeasureUnitId( measureUnitId,paginatedQuery));


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateMeasureUnitConversionCommand model) => Ok(await Mediator.Send(model));


        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteMeasureUnitConversionCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete2([FromQuery] int model) => Ok(await Mediator.Send(new DeleteMeasureUnitConversionCommand(){Id = model}));


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateMeasureUnitConversionCommand model) => Ok(await Mediator.Send(model));

    }
}