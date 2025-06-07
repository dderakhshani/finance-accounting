using Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Commands.Add;
using Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Commands.Delete;
using Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Commands.Update;
using Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Rexp
{
    public class ResourcesExpensesMonthlyController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddMonthlyForecast([FromBody] CreateMonthlyForecastCommand command) => Ok(await Mediator.Send(command));

        [HttpPut]
        public async Task<IActionResult> UpdateMonthlyForecast([FromBody] UpdateMonthlyForecastCommand command) => Ok(await Mediator.Send(command));

        [HttpDelete]
        public async Task<IActionResult> DeleteMonthlyForecast([FromQuery] DeleteMonthlyForecastCommand cmd) => Ok(await Mediator.Send(cmd));

        [HttpGet]
        public async Task<IActionResult> GetMonthlyForecast([FromQuery] GetMonthlyForecatQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetAllMonthlyForecast([FromBody] GetAllMonthlyForecastsQuery query) => Ok(await Mediator.Send(query));

    }
}
