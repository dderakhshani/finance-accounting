using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Bursary.Application.Commands.Cheque.Create;

using Eefa.Bursary.Application.Commands.Cheque.Delete;
using Eefa.Bursary.Application.Commands.Cheque.Update;
using Eefa.Bursary.Application.Queries.Cheque;
using Microsoft.Extensions.Logging;
using MediatR;
using Eefa.Common.Data.Query;
using System;

namespace Eefa.Bursary.WebApi.Controllers
{
    public class ChequeController : ChequeBaseController
    {

        IChequeQueries _chequeQueries;
        ILogger<ChequeController> _logger;

        public ChequeController(IMediator mediator, IChequeQueries chequeQueries,
        ILogger<ChequeController> logger)
        {
            //_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _chequeQueries = chequeQueries ?? throw new ArgumentNullException(nameof(chequeQueries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
 
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateChequeCommand model) => Ok(await Mediator.Send(model));

        [HttpGet]
        public async Task<IActionResult> GetById(int id) => Ok(await _chequeQueries.GetById(id));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _chequeQueries.GetAll(paginatedQuery));


        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteChequeCommand model) => Ok(await Mediator.Send(model));


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateChequeCommand model) => Ok(await Mediator.Send(model));


    }
}
