using Eefa.Bursary.Application.UseCases.Definitions.Bank.Queries;
using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Queries;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers.Definitions
{
    public class BankAccountReferencesController : ApiControllerBase
    {
        public BankAccountReferencesController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetBankAccountReference([FromQuery] GetBankAccountreferenceQuery query) => Ok(await Mediator.Send(query));
        
        [HttpPost]
        public async Task<IActionResult> GetAllBankAccountReferences([FromBody] GetAllBankAccountReferencesQuery query) => Ok(await Mediator.Send(query));


    }
}
