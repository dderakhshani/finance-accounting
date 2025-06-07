using Eefa.Bursary.Application.Queries.FinancialRequest;
using Eefa.Bursary.Application.Queries.TejaratBankAccount;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers
{
    public class TejaratBankAccountController : ApiControllerBase
    {
        ILogger<TejaratBankAccountController> _logger;
        private readonly IGetTejaratBalanceQuery _tejaratBankAccountQueries;

        public TejaratBankAccountController(ILogger<TejaratBankAccountController> logger, IGetTejaratBalanceQuery tejaratBankAccountQueries)
        {
            _logger = logger;
            _tejaratBankAccountQueries = tejaratBankAccountQueries;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetTejaratBalanceQuery query)
        {
            var result = _tejaratBankAccountQueries.GetAll(query);
            return Ok(result);
        }

    }
}
