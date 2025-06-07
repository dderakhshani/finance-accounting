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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using ServiceStack;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class ReportsController : ApiControllerBase
    {
        IReportsQueries _ReportsQueries;

        public ReportsController(

           IReportsQueries ReportsQueries
          )
        {

            _ReportsQueries = ReportsQueries ?? throw new ArgumentNullException(nameof(ReportsQueries));

        }

        [HttpGet]
        public async Task<IActionResult> TejaratReports(string DocumentDate, int DocumentStauseBaseValue)
        {

            var result = await _ReportsQueries.TejaratReports(DocumentDate, DocumentStauseBaseValue);
            return Ok(ServiceResult<List<SpTejaratReportImportCommodity>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllReceiptGroupbyInvoice(DateTime? FromDate,
            DateTime? ToDate,
            int? VoucherHeadId,
            string DocumentIds,
             int? CreditAccountReferenceId,
             int? DebitAccountReferenceId,
             int? CreditAccountHeadId,
             int? CreditAccountReferenceGroupId,
             int? DebitAccountReferenceGroupId,
             int ? DebitAccountHeadId,
            PaginatedQueryModel query)

        {
            var result = await _ReportsQueries.GetAllReceiptGroupInvoice(FromDate, ToDate, null,DocumentIds, CreditAccountReferenceId, DebitAccountReferenceId, CreditAccountHeadId, CreditAccountReferenceGroupId, DebitAccountReferenceGroupId, DebitAccountHeadId, query);


            return Ok(ServiceResult<PagedList<ReceiptGroupbyInvoice>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetCommodityReportsWithWarehouse(DateTime? FromDate,
                DateTime? ToDate,
                string WarehouseId,
                string CodeVoucherGroupId,
                int DocumentStauseBaseValue,
                PaginatedQueryModel query)
        {
            var result = await _ReportsQueries.GetCommodityReportsWithWarehouse(
                FromDate,
                ToDate,
                WarehouseId,
                CodeVoucherGroupId,
                DocumentStauseBaseValue  ,
                 query);
            return Ok(ServiceResult<PagedList<spCommodityReportsWithWarehouse>>.Success(result));
        }
            
        [HttpGet]
        public async Task<IActionResult> GetMonthlyEntryToWarehouse(int? CommodityId, int? WarehouseId, int? YearId, int? EnterMode)
        {

            var result = await _ReportsQueries.GetMonthlyEntryToWarehouse(CommodityId, WarehouseId, YearId, EnterMode);
            return Ok(ServiceResult<List<spGetMonthlyEntryToWarehouse>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetCommodityReports(string FromDate, string ToDate, int? CommodityId, string WarehouseId, string CommodityTitle, PaginatedQueryModel query)
        {
            var result = await _ReportsQueries.GetCommodityReports(FromDate, ToDate, CommodityId, WarehouseId, CommodityTitle, query);
            return Ok(ServiceResult<List<spCommodityReports>>.Success(result));
        }


        [HttpPost]
        public async Task<IActionResult> GetCommodityReportsRial(string FromDate, string ToDate, int? CommodityId, string AccountHeadId, PaginatedQueryModel query)
        {
            var result = await _ReportsQueries.GetCommodityReportsRial(FromDate, ToDate, CommodityId, AccountHeadId, query);
            return Ok(ServiceResult<List<spCommodityReportsRial>>.Success(result));
        }

        
        [HttpPost]
        public async Task<IActionResult> GetContradictionAccounting(string FromDate, string ToDate, int? CommodityId, string AccountHeadId, PaginatedQueryModel query)
        {
            var result = await _ReportsQueries.GetContradictionDebit(FromDate, ToDate, CommodityId, AccountHeadId, query);
            return Ok(ServiceResult<List<spContradictionDebit>>.Success(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> GetCommodityReportsSumAll(string FromDate, string ToDate, int? CommodityId, string WarehouseId, string CommodityTitle, PaginatedQueryModel query)
        {
            var result = await _ReportsQueries.GetCommodityReportsSumAll(FromDate, ToDate, CommodityId, WarehouseId, CommodityTitle, query);
            return Ok(ServiceResult<List<spCommodityReportsSumAll>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetMakeProductPriceReport(string FromDate, string ToDate, PaginatedQueryModel query)
        {
            var result = await _ReportsQueries.GetMakeProductPriceReport(FromDate, ToDate, query);
            return Ok(ServiceResult<PagedList<MakeProductPrice>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetCommodityReceiptReports(DateTime FromDate, DateTime ToDate, int? CommodityId, int? WarehouseId, int? DocumentNo, int? RequestNo, string CommodityTitle, PaginatedQueryModel query)
        {
            var result = await _ReportsQueries.GetCommodityReceiptReports(FromDate, ToDate, CommodityId, WarehouseId, DocumentNo, RequestNo,!String.IsNullOrEmpty(CommodityTitle)? CommodityTitle.Replace("%_","%"):String.Empty, query);
            return Ok(ServiceResult<List<spCommodityReceiptReports>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetCommodityReceiptReportsRial(string FromDate, string ToDate, int? CommodityId, int? AccountHeadId, int? WarehouseId, int? DocumentNo, PaginatedQueryModel query)
        {
            
            var result = await _ReportsQueries.GetCommodityReceiptReportsRial(FromDate, ToDate, CommodityId, AccountHeadId, WarehouseId, DocumentNo, query);
            return Ok(ServiceResult<List<spCommodityReceiptReports>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllInvoice(string FromDate, string ToDate, string InvoiceNo, int? CreditAccountReferenceId, PaginatedQueryModel query)
        {
            var result = await _ReportsQueries.GetAllInvoice(FromDate, ToDate, InvoiceNo, CreditAccountReferenceId, query);
            return Ok(ServiceResult<List<spReceiptInvoice>>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetDocumentItemForTadbir(string FromDate, string ToDate, int? CodeVoucherGroupId, int? DocumentNo, int? WarehouseId)
        {

            var result = await _ReportsQueries.GetDocumentItemForTadbir(FromDate, ToDate, CodeVoucherGroupId, DocumentNo, WarehouseId);
            return Ok(ServiceResult<List<spGetDocumentItemForTadbir>>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetWarehouseLayoutQuantitiesByTadbir(int warehouseId,int YearId)
        {

            var result = await _ReportsQueries.GetReportControlsWarehouseLayoutQuantitiesByTadbir(warehouseId, YearId);
            return Ok(ServiceResult<List<spReportControlsWarehouseLayoutQuantitiesByTadbir>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> UploadInventory()
        {

            var httpRequest = HttpContext.Request.Form;
            var file = httpRequest.Files[0];
            var warehouseId = httpRequest["warehouseId"];
            var YearId = httpRequest["YearId"];
            var result = await _ReportsQueries.UploadInventoryTadbir(file, warehouseId, YearId);
            return Ok(ServiceResult<List<spReportControlsWarehouseLayoutQuantitiesByTadbir>>.Success(result));

        }
        [HttpPost]
        public async Task<IActionResult> GetFreightPays(string FromDate, string ToDate, int? AccountReferenceId, PaginatedQueryModel query)
        {
            var result = await _ReportsQueries.GetFreightPays(FromDate, ToDate, AccountReferenceId, query);
            return Ok(ServiceResult<PagedList<Stp_FreightPays>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetCommodityCost(string FromDate, string ToDate,  string warehouseId, PaginatedQueryModel query)
        {
            var result = await _ReportsQueries.GetCommodityCost(FromDate, ToDate, warehouseId, query);
            return Ok(ServiceResult<PagedList<spCommodityCost>>.Success(result));
        }

    }
}