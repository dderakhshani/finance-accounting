using Eefa.Bursary.Application.UseCases.Calendar.DateConverts.Commands;
using Eefa.Bursary.Application.UseCases.Calendar.DateConverts.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Calendar
{
    public class CalendarController : ApiControllerBase
    {
        [HttpPut]
        public async Task<IActionResult> SetHoliday([FromBody] SetHolidaysCommand command) => Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> GetDate([FromQuery] GetDateQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetDatesList([FromBody] GetAllDatesQuery query) => Ok(await Mediator.Send(query));


    }
}
