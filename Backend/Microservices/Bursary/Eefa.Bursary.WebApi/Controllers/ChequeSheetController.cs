using Eefa.Bursary.Application.Queries.ChequeSheet;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Eefa.Bursary.Application.Commands.ChequeSheet;
using Eefa.Bursary.Application.Commands.ChequeSheet.Update;


namespace Eefa.Bursary.WebApi.Controllers
{

    public class ChequeSheetController : ApiControllerBase
    {
        private readonly IChequeSheetQueries _chequeSheetQueries;
        ILogger<ChequeSheetController> _logger;
        public ChequeSheetController(ILogger<ChequeSheetController> logger, IChequeSheetQueries chequeSheetQueries)
        {

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _chequeSheetQueries = chequeSheetQueries;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateChequeSheetCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _chequeSheetQueries.GetAll(paginatedQuery));

        [HttpPost]
        public async Task<IActionResult> GetUsedCheques([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _chequeSheetQueries.GetUsedCheques(paginatedQuery));

        [HttpPost]
        public async Task<IActionResult> SetDocumentsForCheque([FromBody] CreateChequeSheetDocumentCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> AddFetchChequeSheets([FromBody] CreateChequeSheetCommand model)
        {
            await Mediator.Send(model);
            var conditions = new PaginatedQueryModel();
            return Ok(await _chequeSheetQueries.GetAll(conditions));
        }

        [HttpPost]
        public async Task<IActionResult> EditReciptChequeSheet([FromBody] UpdateChequeSheetCommand model)=>  Ok(await Mediator.Send(model));
        
        [HttpGet]
        public async Task<IActionResult> GetChequeSheetById(int chequeId) => Ok(await _chequeSheetQueries.GetChequeSheetById(chequeId));



    }
}
