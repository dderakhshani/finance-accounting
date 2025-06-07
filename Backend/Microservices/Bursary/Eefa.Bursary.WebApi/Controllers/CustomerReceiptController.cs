using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Eefa.Bursary.Application.Commands.CustomerReceipt.Create;
using Eefa.Bursary.Application.Commands.CustomerReceipt.Update;
using Eefa.Bursary.Application.Queries.CustomerReceipt;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.Extensions.Logging;
using Eefa.Bursary.Application.Commands.CustomerReceipt.Delete;

using Eefa.Common;
using Eefa.Common.Common.Attributes;
//using Eefa.Bursary.Application.Commands.CreateByExcel.Create;

namespace Eefa.Bursary.WebApi.Controllers
{

    public class CustomerReceiptController : ApiControllerBase
    {
        ILogger<CustomerReceiptController> _logger;
        private readonly ICustomerReceiptQueries _customerReceiptQueries;

       

        public CustomerReceiptController(
            ILogger<CustomerReceiptController> logger, ICustomerReceiptQueries customerReceiptQueries)
        {

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _customerReceiptQueries = customerReceiptQueries;
          
        }
        [ClickRateLimiter(allowedClickIntervalSeconds: 3, allowedClickCount: 1)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCustomerReceiptCommand model)
        {
              return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _customerReceiptQueries.GetAll(paginatedQuery));

        [HttpPost]
        public async Task<IActionResult> GetUploadedReceipts([FromBody] string urlAddress) => Ok(_customerReceiptQueries.GetUploadedReceipts(urlAddress));


        [HttpPost]

        public async Task<IActionResult> AddDocumentForBursaryArticles([FromBody] CreateDocumentForBursaryArticlesCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerReceiptCommand model)
        {
            var result = await Mediator.Send(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVoucherHeadId([FromBody] UpdateVoucherHeadIdCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        public async Task<IActionResult> UpdateIsPending([FromBody] UpdateIsPendingCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteCustomerReceiptCommand model) => Ok(await Mediator.Send(model));

        //[HttpPost]
        //public async Task<IActionResult> AddByExcel([FromBody] CreateByExcelCommand model) => Ok(await Mediator.Send(model));
    }
}
