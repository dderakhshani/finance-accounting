using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Eefa.Bursary.Application.Queries.AccountReferenceGroup;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Eefa.Bursary.WebApi.Controllers
{
    public class AccountReferenceGroupController : ApiControllerBase
    {
        IAccountReferencesGroupQueries _accountReferenceGroupQueries;
        ILogger<AccountReferenceController> _logger;
        public AccountReferenceGroupController(IMediator mediator, ILogger<AccountReferenceController> logger, IAccountReferencesGroupQueries accountReferenceGroupQueries)
        {
             
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accountReferenceGroupQueries = accountReferenceGroupQueries;
           // _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _accountReferenceGroupQueries.GetAll(paginatedQuery));

        [HttpGet]
        public async Task<IActionResult> GetReferencesGroupBy(int id) => Ok(await _accountReferenceGroupQueries.GetReferenceGroupsBy(id));
    }
}
