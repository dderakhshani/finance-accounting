using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Queries;
using Eefa.Bursary.Application.UseCases.Definitions.CurrencyType.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Definitions
{
    public class CurrencyTypesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetCurrencyTypeQuerry query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllCurrencyTypeQuerry query) => Ok(await Mediator.Send(query));

    }
}
