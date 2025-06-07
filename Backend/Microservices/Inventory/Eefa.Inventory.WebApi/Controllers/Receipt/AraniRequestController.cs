using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Exceptions;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Eefa.Inventory.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers.Receipt
{
    public class AraniRequestController : ApiControllerBase
    {
        IAraniRequstCommandQueries _receiptRepository;
        public AraniRequestController(IAraniRequstCommandQueries receiptQueries)
        {
            _receiptRepository = receiptQueries ?? throw new ArgumentNullException(nameof(receiptQueries));
              
        }


        [HttpGet]
        public async Task<IActionResult> leavingWarehouseRequestById(int requestId,int warehouseId)
        {

            var result = await _receiptRepository.leavingWarehouseRequestById(requestId, warehouseId);
            return Ok(ServiceResult<RequestCommodityWarehouseModel>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetPurchaseRequestById(int requestId)
        {

            //درخواست خرید در سیستم ایفا تجارت وجود نداشت در سیستم آرانی دنبال آن بگردد
            var request = await _receiptRepository.GetPurchaseRequestById(requestId);

            if (request != null)
            {
                return Ok(ServiceResult<ReceiptQueryModel>.Success(request));
            }
            //جستجو در بازگشتی ها به انبار از سیستم آرانی
            else if (request == null)
            {
                var result = await _receiptRepository.GetReturnCommodityWarehouse(requestId);
                if (result != null)
                {
                    return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
                }
                else
                {
                    throw new ValidationError("شماره درخواست اشتباه است");
                }
            }

            return null;
        }
        [HttpPost]
        public async Task<IActionResult> GetErpDarkhastJoinDocumentHeads(

            DateTime FromDate,
            DateTime ToDate,
            string RequestNo,
            PaginatedQueryModel paginatedQuery)
        {
            var result = await _receiptRepository.GetErpDarkhastJoinDocumentHeads(FromDate, ToDate, RequestNo, paginatedQuery);

            return Ok(ServiceResult<PagedList<spErpDarkhastJoinDocumentHeads>>.Success(result));
        }
        

        

    }
}