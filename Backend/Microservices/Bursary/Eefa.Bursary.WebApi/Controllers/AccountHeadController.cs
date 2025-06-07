using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Eefa.Bursary.Application.Queries.AccountHead;
using Eefa.Common.Web;
using MediatR;
using Eefa.Common.Data.Query;
using Microsoft.Extensions.Logging;

namespace Eefa.Bursary.WebApi.Controllers
{
    public class AccountHeadController : ApiControllerBase
    {
          IAccountHeadQueries _accountHeadQueries;
          ILogger<AccountReferenceController> _logger;
        public AccountHeadController(IMediator mediator, IAccountHeadQueries accountHeadQueries, ILogger<AccountReferenceController> logger)
        {
            _accountHeadQueries = accountHeadQueries  ?? throw new ArgumentNullException(nameof(accountHeadQueries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
         //   _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ResponseCache(Duration = 36000)]
        public async Task<IActionResult> GetAll([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _accountHeadQueries.GetAll(paginatedQuery));

    }
}
