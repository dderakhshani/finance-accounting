using Eefa.Bursary.Application.UseCases.Definitions.ChequeType;
using Eefa.Bursary.Application.UseCases.Definitions.CurrencyType.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Definitions
{
    public class ChequetypesController: ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetChequeTypeQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllChequeTypeQuery query) => Ok(await Mediator.Send(query));

    }
}
