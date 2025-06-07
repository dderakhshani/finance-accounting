using Eefa.Bursary.Application.UseCases.Rexp.Definitions.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Rexp
{
    public class ResorcesExpensesController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ResorcesExpensesList([FromBody] GetAllResourcesExpensesQuery query) => Ok(await Mediator.Send(query));
    }
}
