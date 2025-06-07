using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Bursary.Application.Queries.FinancialRequest;
using Eefa.Common.Data.Query;
using Microsoft.Extensions.Logging;
using Eefa.Bursary.Application.Models;

namespace Eefa.Bursary.WebApi.Controllers
{
    /// <summary>
    /// FinancalRequestControlller
    /// </summary>
    public class FinancialRequestController : ApiControllerBase
    {
        ILogger<CustomerReceiptController> _logger;
        private readonly IFinancialRequestQueries _financialRequestQueries;

        /// <summary>
        /// FinancialRequest Contractor
        /// </summary>
        /// <param name="financialRequestQueries"></param>
        /// <param name="logger"></param>
        public FinancialRequestController(IFinancialRequestQueries financialRequestQueries, ILogger<CustomerReceiptController> logger)
        {
            _financialRequestQueries = financialRequestQueries;
            _logger = logger;
            
        }

        /// <summary>
        /// Get All FinancialRequest (Egger Loading)
        /// </summary>
        /// <param name="paginatedQuery"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await _financialRequestQueries.GetAll(paginatedQuery));

        /// <summary>
        /// Get Financial Request And Details By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetById(int id) => Ok(await _financialRequestQueries.GetById(id));

        [HttpGet]
        public async Task<IActionResult> GetDetailsByFinancialRequestId(int financialRequestId) => Ok(await _financialRequestQueries.GetDetailsByFinancialRequestId(financialRequestId));

        [HttpGet]
        public async Task<IActionResult> GetAttachmentsByFinancialRequestId(int financialRequestId) => Ok(await _financialRequestQueries.GetAttachmentsByFinancialRequestId(financialRequestId));

        [HttpGet]
        public async Task<IActionResult> GetLastReceiptInfo()
        {
           var result = Ok(await _financialRequestQueries.GetLastReceiptInfo());
            return result;
        }




        [HttpGet]
        public async Task<IActionResult> GetReqeustCountByStatus(int status)
        {
            var result = Ok(await _financialRequestQueries.GetReqeustCountByStatus(status));
            return result;
        }





        [HttpPost]
        public async Task<IActionResult> SetDocumentsForBursaryPaymentArticles([FromBody] SendDocument model) => Ok(await _financialRequestQueries.SetDocumentsForBursaryPaymentArticles(model));
      
    }
}
