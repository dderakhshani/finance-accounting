using Eefa.Bursary.Application.Queries.AccountReference;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers
{
    public class AccountReferenceController : ApiControllerBase
    {
        IAccountReferenceQueries _accountReferenceQueries;
        ILogger<AccountReferenceController> _logger;



        public AccountReferenceController(IMediator mediator, IAccountReferenceQueries accountReferenceQueries,
    ILogger<AccountReferenceController> logger)
        {
            //_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _accountReferenceQueries = accountReferenceQueries ?? throw new ArgumentNullException(nameof(accountReferenceQueries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        [HttpGet]
        public async Task<IActionResult> GetById(int id) => Ok(await _accountReferenceQueries.GetById(id));

        [HttpGet]
        public async Task<IActionResult> ReferenceAccountsByGroupId(int id) => Ok(await _accountReferenceQueries.ReferenceAccountsByGroupId(id));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _accountReferenceQueries.GetAll(paginatedQuery));







    }
}
