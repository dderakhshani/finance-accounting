using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data.Query;
using Eefa.Common.Data;
using Eefa.Common.Web;

using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers.Receipt
{
    public class TemporaryReceiptController : ApiControllerBase
    {
        ITemporaryReceipQueries _receiptRepository;
        public TemporaryReceiptController(ITemporaryReceipQueries receiptQueries)
        {
            _receiptRepository = receiptQueries ?? throw new ArgumentNullException(nameof(receiptQueries));
              
        }
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {

            var result = await _receiptRepository.GetById(id);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetAll(
            int? DocumentStauseBaseValue,
            string FromDate,
            string ToDate,
            PaginatedQueryModel paginatedQuery)
        {
            var result = await _receiptRepository.GetAll(DocumentStauseBaseValue, FromDate, ToDate, paginatedQuery);

            return Ok(ServiceResult<PagedList<ReceiptQueryModel>>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetByDocumentNoWithWarehouseId(int DocumentNo, int warehouseId)
        {

            var result = await _receiptRepository.GetByDocumentNo(DocumentNo, warehouseId);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetByDocumentNo(int DocumentNo)
        {

            var result = await _receiptRepository.GetByDocumentNo(DocumentNo);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetByRequesterReferenceId(int RequesterReferenceId, string FromDate, string ToDate)
        {

            var result = await _receiptRepository.GetByRequesterReferenceId(RequesterReferenceId, FromDate, ToDate);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }
        

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateReceiptCommand model) => Ok(await Mediator.Send(model));
        
        [HttpPost]
        public async Task<IActionResult> AddSingleRow([FromBody] CreateSingleRowCommand model) => Ok(await Mediator.Send(model));


        [HttpDelete]
        public async Task<IActionResult> Archive(int id, int codeVoucherGroupId)
        {

            return Ok(await Mediator.Send(new ArchiveReceiptCommand() { Id = id}));
        }
        [HttpPost]
        public async Task<IActionResult> update([FromBody] UpdateReceiptCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> CreateInputProduct([FromBody] CreateProductCommand model) => Ok(await Mediator.Send(model));


    }
}