using Eefa.Bursary.Application.Queries.DocumentHead;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers
{
    [Route("api/bursary/[controller]/[action]")]
    public class DocumentHeadController : ApiControllerBase
    {
        IDocumentHeadQueries _documentHeadQueries;
        ILogger<DocumentHeadController> _logger;

        public DocumentHeadController(IMediator mediator, IDocumentHeadQueries documentHeadQueries,
            ILogger<DocumentHeadController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _documentHeadQueries = documentHeadQueries ?? throw new ArgumentNullException(nameof(documentHeadQueries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [HttpPost]
        public async Task<IActionResult> GetDocumentHeadsByReferenceId([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _documentHeadQueries.GetDocumentHeadsByReferenceId(paginatedQuery));






    }
}
