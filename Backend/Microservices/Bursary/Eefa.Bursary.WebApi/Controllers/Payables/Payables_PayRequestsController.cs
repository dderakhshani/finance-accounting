using Eefa.Bursary.Application.UseCases.Payables.PayOrders.Queries;
using Eefa.Bursary.Application.UseCases.Payables.PayRequests.Queries;
using Eefa.Common.Web;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Payables
{
    public class Payables_PayRequestsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPayRequestQuery query) => Ok(await Mediator.Send(query));
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllPayRequestsQuery query) => Ok(await Mediator.Send(query));

    }
}
