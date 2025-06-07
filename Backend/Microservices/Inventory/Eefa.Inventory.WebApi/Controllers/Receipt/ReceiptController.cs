using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Eefa.Inventory.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Eefa.Invertory.Infrastructure.Context;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Application.Commands.Operations.UpdateReadStatusRecipt;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.WebApi.Controllers.Receipt
{
    public class ReceiptController : ApiControllerBase
    {
        IReceiptQueries _receiptQueries;
        IReceiptCommandsService _receiptCommandsService;
        ICurrentUserAccessor _currentUserAccessor;



        public ReceiptController(IReceiptQueries receiptQueries,
            IAccountReferences accountReferences,
            IReceiptCommandsService receiptCommandsService,
            ICurrentUserAccessor currentUserAccessor
            )
        {
            _receiptQueries = receiptQueries ?? throw new ArgumentNullException(nameof(receiptQueries));
            _receiptCommandsService = receiptCommandsService;
            _currentUserAccessor = currentUserAccessor;

        }

        [HttpPost]
        public async Task<IActionResult> GetByListId(ListIdModel model)
        {

            var result = await _receiptQueries.GetByListId(model.ListIds);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }
        

        [HttpPost]
        public async Task<IActionResult> GetAll(
            int? DocumentStauseBaseValue,
            bool? IsImportPurchase,
            string FromDate,
            string ToDate,
            PaginatedQueryModel paginatedQuery)
        {
            var result = await _receiptQueries.GetAll(DocumentStauseBaseValue, IsImportPurchase, FromDate, ToDate, paginatedQuery);

            return Ok(ServiceResult<PagedList<ReceiptQueryModel>>.Success(result));
        }
        /// <summary>
        /// فهرست جامع
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="paginatedQuery"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetComprehensiveList(

            string FromDate,
            string ToDate,
            PaginatedQueryModel paginatedQuery)
        {
            var result = await _receiptQueries.GetComprehensiveList(FromDate, ToDate, paginatedQuery);

            return Ok(ServiceResult<PagedList<ReceiptQueryModel>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllReceiptItemsCommodity(
            int? DocumentStauseBaseValue,
            string FromDate,
            string ToDate,
            PaginatedQueryModel paginatedQuery)
        {
            var result = await _receiptQueries.GetAllReceiptItemsCommodity(DocumentStauseBaseValue, FromDate, ToDate, paginatedQuery);

            return Ok(ServiceResult<PagedList<ReceiptQueryModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetAllAmountRecipt(

            string FromDate,
            string ToDate,
            PaginatedQueryModel paginatedQuery)
        {
            var result = await _receiptQueries.GetAllAmountReceipt(FromDate, ToDate, paginatedQuery);

            return Ok(ServiceResult<PagedList<ReceiptQueryModel>>.Success(result));
        }


        [HttpPost]
        public async Task<IActionResult> GetByViewId(
            int ViewId,
            string FromDate,
            string ToDate,
            PaginatedQueryModel paginatedQuery)
        {
            var result = await _receiptQueries.GetByViewId(ViewId, FromDate, ToDate, paginatedQuery);

            return Ok(ServiceResult<PagedList<ReceiptQueryModel>>.Success(result));
        }
        /// <summary>
        /// درخواست دریافت کالا
        /// </summary>
        /// <param name="DocumentStauseBaseValue"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="paginatedQuery"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetByDocumentStauseBaseValue(
            int DocumentStauseBaseValue,
            string FromDate,
            string ToDate,
            PaginatedQueryModel paginatedQuery)
        {
            var result = await _receiptQueries.GetByDocumentStatuesBaseValue(DocumentStauseBaseValue, FromDate, ToDate, paginatedQuery);

            return Ok(ServiceResult<PagedList<ReceiptQueryModel>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> PlacementWarehouseDirectReceipt([FromBody] PlacementWarehouseDirectReceiptCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> UpdateStatusDirectReceipt([FromBody] UpdateStatusDirectReceiptCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> update([FromBody] UpdateReceiptCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> ConvertToRialsReceipt([FromBody] ConvertToRailsReceiptCommand model) => Ok(await Mediator.Send(model));


        [HttpPost]
        public async Task<IActionResult> ConvertToMechanizedDocument([FromBody] ConvertToMechanizedDocumentCommand model) => Ok(await Mediator.Send(model));
        /// <summary>
        /// درخواست ویرایش سند ارسال شده به حسابداری
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCorrectionRequest([FromBody] CreateCorrectionRequestCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> UpdateCorrectionRequest([FromBody] UpdateCorrectionRequestCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateStatusReceipt([FromBody] UpdatePrintStatusReceiptCommand model) => Ok(await Mediator.Send(model));
        
        [HttpPost]
        public async Task<IActionResult> UpdateExtraCost([FromBody] UpdateExteraCostCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateWarehouseCardex([FromBody] UpdateWarehouseCardexCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateIsPlacementCompleteReceipt([FromBody] UpdateIsPlacementCompleteCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdatePrintCountReceipt([FromBody] UpdatePrintCountCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateDocumentItemsBomQuantity([FromBody] UpdateDocumentItemsBomQuantityCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> AddDocumentItemsBomQuantity([FromBody] AddDocumentItemsBomQuantityCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> SplitCommodityQuantity([FromBody] SplitCommodityQuantityCommand model) => Ok(await Mediator.Send(model));


        
        [HttpPost]
        public async Task<IActionResult> GetAllReceiptItems(string FromDate, string ToDate, PaginatedQueryModel paginatedQuery)
        {
            var result = await _receiptQueries.GetAllReceiptItems(FromDate, ToDate, paginatedQuery);

            return Ok(ServiceResult<PagedList<WarehouseHistoriesDocumentViewModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> CreateStartDocument([FromBody] CreateStartDocumentCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateStartDocument([FromBody] UpdateStartDocumentItemsCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> CreateMakeProductPrice([FromBody] CreateMakeProductPriceCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> UpdateIsDocumentIssuance([FromBody] UpdateIsDocumentIssuanceCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> ArchiveDocumentHeadsByDocumentDate([FromBody] ArchiveDocumentHeadsByDocumentDateCommand model) => Ok(await Mediator.Send(model));
        
        [HttpPost]
        public async Task<IActionResult> InsertDocumentHeads([FromBody] InsertDocumentHeadsCommand model) => Ok(await Mediator.Send(model));
        /// <summary>
        /// ویرایش میانگین قیمت براساس رسید
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateAvgPriceAfterChangeBuyPrice([FromBody] UpdateAvgPriceAfterChangeBuyPriceCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        public async Task<IActionResult> GetWarehouseRequestExitView(string FromDate, string ToDate,PaginatedQueryModel query)
        {
            var result = await _receiptQueries.GetWarehouseRequestExitView(FromDate, ToDate, query);
            return Ok(ServiceResult<PagedList<WarehouseRequestExitViewModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GeView_Sina_FinancialOperationNumber(string FromDate, string ToDate, PaginatedQueryModel query)
        {
            var result = await _receiptQueries.GeView_Sina_FinancialOperationNumber(query);
            return Ok(ServiceResult<PagedList<View_Sina_FinancialOperationNumberModel>>.Success(result));

        }
        [HttpPost]
        public async Task<IActionResult> GetReceiptGroupInvoice(DateTime? FromDate,
            DateTime? ToDate,
            int? VoucherHeadId,
            string DocumentIds,
             int? CreditAccountReferenceId,
             int? DebitAccountReferenceId,
             int? CreditAccountHeadId,
             int? CreditAccountReferenceGroupId,
             int? DebitAccountReferenceGroupId,
             int? DebitAccountHeadId,
            PaginatedQueryModel query)

        {
            var result = await _receiptQueries.GetAllReceiptGroupInvoice(FromDate, ToDate, null, DocumentIds, CreditAccountReferenceId, DebitAccountReferenceId, CreditAccountHeadId, CreditAccountReferenceGroupId, DebitAccountReferenceGroupId, DebitAccountHeadId, query);


            return Ok(ServiceResult<PagedList<SpReceiptGroupbyInvoice>>.Success(result));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateReadStatusReceipt([FromBody] UpdateReadStatusReceiptCommand model) => Ok(await Mediator.Send(model));
        //====================================================================================

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {

            var result = await _receiptQueries.GetById(id);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetByInvoiceNo(string InvoiceNo, string Date, int? CreditAccountReferenceId)
        {

            var result = await _receiptQueries.GetByInvoiceNo(InvoiceNo, Date, CreditAccountReferenceId);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetPlacementById(int id, int? WarehouseId)
        {

            var result = await _receiptQueries.GetPlacementById(id, WarehouseId);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetByDocumentNoAndCodeVoucherGroupId(int DocumentNo, int codeVoucherGroupId)
        {
            var result = await _receiptQueries.GetByDocumentNoAndCodeVoucherGroupId(DocumentNo, codeVoucherGroupId);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetByDocumentNoAndDocumentStauseBaseValue(int DocumentNo, int DocumentStauseBaseValue)
        {

            var result = await _receiptQueries.GetByDocumentNoAndDocumentStauseBaseValue(DocumentNo, DocumentStauseBaseValue);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetDocumentItemsBomById(int Id)
        {

            var result = await _receiptQueries.GetDocumentItemsBomById(Id);
            return Ok(ServiceResult<ReceiptItemModel>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetALLDocumentItemsBom(int DocumentItemsId)
        {

            var result = await _receiptQueries.GetALLDocumentItemsBom(DocumentItemsId);
            return Ok(ServiceResult<PagedList<ReceiptItemModel>>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetDocumentAttachment(int DocumentHeadId)
        {

            var result = await _receiptQueries.GetDocumentAttachmentIdByDocumentIdId(DocumentHeadId);
            return Ok(ServiceResult<int[]>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetByDocumentId(int DocumentId)
        {

            var result = await _receiptQueries.GetByDocumentId(DocumentId);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetByDocumentIdByDocumentHeadId(int VoucherHeadId)
        {

            var result = await _receiptQueries.GetByDocumentIdByVoucherHeadId(VoucherHeadId);
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetCorrectionRequestById(int Id)
        {

            var result = await _receiptQueries.GetCorrectionRequestById(Id);
            return Ok(ServiceResult<CorrectionRequest>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> MechanizedDocumentEditing([FromBody] ConvertToRailsReceiptCommand model)
        {
            return await UpdateAccountingDocument(model);


        }

        
        [HttpPost]
        public async Task<IActionResult> UpdateAccountingDocument([FromBody] ConvertToRailsReceiptCommand model)
        {
            var result = await _receiptQueries.GetCorrectionRequestById(Convert.ToInt32(model.CorrectionRequestId));
            if (result == null && result.Status != 0)
            {
                throw new ValidationError("این درخواست از لیست درخواست های در انتظار پردازش خارج شده است");
            }
            var OldData = Newtonsoft.Json.JsonConvert.DeserializeObject<ConvertToRailsReceiptCommand>(result.OldData);

            UpdateCorrectionRequestCommand update = new UpdateCorrectionRequestCommand();
            update.Id = result.Id;
            update.VerifierUserId = _currentUserAccessor.GetId();
            update.DocumentId = Convert.ToInt32(model.DocumentId);
            OldData.DocumentId = Convert.ToInt32(model.DocumentId);
            new LogWriter("Start UpdateAccountingDocument ===================================== DocumentId" + model.DocumentId.ToString());

            var Response = await Mediator.Send(model);


            if (Response.Succeed)
            {
                new LogWriter("Succeed UpdateCorrectionRequestCommand ===================================== DocumentId=" + model.DocumentId.ToString());
                var ResultAPi = await _receiptCommandsService.UpdateVoucher(model);
                if (ResultAPi.succeed)
                {
                    new LogWriter("after CallApiAutoVoucher2  ===================================== DocumentId=" + model.DocumentId.ToString());
                    //اگر تعداد کالا تغییر کرده باشد
                    foreach (var documentItem in model.ReceiptDocumentItems)
                    {
                        var _Quantity = OldData.ReceiptDocumentItems.Select(a => a.Quantity).FirstOrDefault();
                        if (documentItem.Quantity != _Quantity)
                        {
                            UpdateQuantityCommand UpdateQuantityCommand = new UpdateQuantityCommand() { Id = documentItem.Id, Quantity = documentItem.Quantity };
                            await Mediator.Send(UpdateQuantityCommand);



                            new LogWriter("after UpdateQuantityCommand  ===================================== DocumentId=" + model.DocumentId.ToString() + " Quantity=" + documentItem.Quantity.ToString());
                        }

                    }

                    update.Status = 1;
                    update.Description = "عملیات ویرایش با موفقیت انجام شد";

                    await Mediator.Send(update);
                }
                else
                {
                    update.Status = 0;
                    update.Description = $"{ResultAPi.errors[0].message} مشکل در صدور مجددا سند مکانیزه";
                    await Mediator.Send(update);
                    await Mediator.Send(OldData);
                    throw new ValidationError(ResultAPi.errors[0].message);
                }
            }
            else
            {
                update.Status = 0;
                update.Description = "مشکل در بروزرسانی اطلاعات رسید مالی";
                await Mediator.Send(OldData);
                await Mediator.Send(update);
                throw new ValidationError(update.Description);

            }

            return Ok(ServiceResult<CorrectionRequest>.Success(null));
        }

        

        [HttpGet]
        public async Task<IActionResult> GetCostImportReceipts(int ReferenceId)
        {

            var result = await _receiptQueries.GetCostImportReceipts(ReferenceId);
            return Ok(ServiceResult<long>.Success(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetStartReceiptsItem(int CommodityId, int WarehouseId)
        {

            var result = await _receiptQueries.GetStartReceiptsItem(CommodityId, WarehouseId);
            return Ok(ServiceResult<DocumentItem>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> WarehouseCheckLoseData()
        {

            var result = await _receiptCommandsService.WarehouseCheckLoseData();
            return Ok();
        }
        
    }




}
