using Eefa.Bursary.Application.UseCases.Rexp.DailyForeCasts.Commands.Calcs;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Rexp
{
    public class ResourcesExpensesDailyController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CakculateDayForecast([FromBody] CalculateDailyForecast command) => Ok(await Mediator.Send(command));

    }

}
