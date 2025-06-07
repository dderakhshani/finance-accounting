using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Queries;
using Eefa.Bursary.Application.UseCases.Payables.PayOrders.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Payables
{
    public class Payables_PayOrdersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPayOrderQuery query) => Ok(await Mediator.Send(query));
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllPayOrdersQuery query) => Ok(await Mediator.Send(query));

    }
}
