
using Eefa.Bursary.Application.Queries.FinancialRequest;
using Eefa.Common.Data.Query;
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
    public class _FinancialRequestController : FinancialRequestBaseController
    {
        IFinancialRequestQueries _financialRequestQueries;
        ILogger<_FinancialRequestController> _logger;

        public _FinancialRequestController(IMediator mediator, IFinancialRequestQueries financialRequestQueries,
            ILogger<_FinancialRequestController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _financialRequestQueries = financialRequestQueries ?? throw new ArgumentNullException(nameof(financialRequestQueries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateFinancialRequestCommand model) => Ok(await Mediator.Send(model));

        [HttpGet]
        public async Task<IActionResult> GetById(int id) => Ok(await _financialRequestQueries.GetById(id));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _financialRequestQueries.GetAll(paginatedQuery));


        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteFinancialRequestCommand model) => Ok(await Mediator.Send(model));


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateFinancialRequestCommand model) => Ok(await Mediator.Send(model));




    }
}
