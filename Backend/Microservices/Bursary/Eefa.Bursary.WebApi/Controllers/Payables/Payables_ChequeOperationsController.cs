using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Queries;
using Eefa.Bursary.Application.UseCases.Payables.ChequeOperations.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Payables
{
    public class Payables_ChequeOperationsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> GetPayableChequesOperationsList([FromBody] GetAllOperationsListQuery query) => Ok(await Mediator.Send(query));
        [HttpPost]
        public async Task<IActionResult> GetOperationsChainList([FromBody] GetAllOperationsChainListQuery query) => Ok(await Mediator.Send(query));

    }
}
