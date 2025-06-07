using Eefa.Bursary.Application.UseCases.Definitions.PayTypes.Queries;
using Eefa.Bursary.Application.UseCases.Payables.PayRequests.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Payables
{
    public class Payables_PayTypesController:ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPayTypeQuery query) => Ok(await Mediator.Send(query));
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllPayTypesQuery query) => Ok(await Mediator.Send(query));

    }
}
