using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;

namespace Eefa.Inventory.Application
{
    public interface IReportsQueries : IQuery
    {
        Task<List<SpTejaratReportImportCommodity>> TejaratReports(string DocumentDate, int DocumentStatuesBaseValue);
        Task<PagedList<ReceiptGroupbyInvoice>> GetAllReceiptGroupInvoice(DateTime? FromDate,
            DateTime? ToDate,
            int? VoucherHeadId,
            string DocumentIds,
             int? CreditAccountReferenceId,
             int? DebitAccountReferenceId,
             int? CreditAccountHeadId,
             int? CreditAccountReferenceGroupId,
             int? DebitAccountReferenceGroupId,
             int? DebitAccountHeadId,

            PaginatedQueryModel query);
        Task<PagedList<spCommodityReportsWithWarehouse>> GetCommodityReportsWithWarehouse(DateTime? FromDate,
          DateTime? ToDate,
           string WarehouseId,
           string CodeVoucherGroupId,
           int DocumentStauseBaseValue,
          PaginatedQueryModel query);
        Task<List<spGetMonthlyEntryToWarehouse>> GetMonthlyEntryToWarehouse(int? CommodityId, int? WarehouseId, int? YearId, int? EnterMode);
        Task<List<spCommodityReports>> GetCommodityReports(string FromDate, string ToDate, int? CommodityId, string WarehouseId, string CommodityTitle, PaginatedQueryModel query);
        Task<List<spCommodityReportsRial>> GetCommodityReportsRial(string FromDate, string ToDate, int? CommodityId, string AccountHeadId, PaginatedQueryModel query);
        Task<List<spCommodityReportsSumAll>> GetCommodityReportsSumAll(string FromDate, string ToDate, int? CommodityId, string WarehouseId, string CommodityTitle, PaginatedQueryModel query);
        Task<List<spCommodityReceiptReports>> GetCommodityReceiptReports(DateTime FromDate, DateTime ToDate, int? CommodityId, int? WarehouseId, int? DocumentNo, int? RequestNo, string CommodityTitle, PaginatedQueryModel query);
        Task<List<spReceiptInvoice>> GetAllInvoice(string FromDate, string ToDate, string InvoiceNo, int? CreditAccountReferenceId, PaginatedQueryModel query);
        //-----------------------------Tadbir----------------------------------------------------------
        Task<List<spGetDocumentItemForTadbir>> GetDocumentItemForTadbir(string FromDate, string ToDate, int? CodeVoucherGroupId, int? DocumentNo, int? WarehouseId);
        Task<List<spReportControlsWarehouseLayoutQuantitiesByTadbir>> UploadInventoryTadbir(IFormFile file, string warehouseId, string yearId);
        Task<List<spReportControlsWarehouseLayoutQuantitiesByTadbir>> GetReportControlsWarehouseLayoutQuantitiesByTadbir(int warehouseId, int yearId);
        Task<PagedList<Stp_FreightPays>> GetFreightPays(string FromDate, string ToDate, int? AccountReferenceId, PaginatedQueryModel query);
        Task<PagedList<spCommodityCost>> GetCommodityCost(string FromDate, string ToDate, string warehouseId, PaginatedQueryModel query);
        Task<PagedList<MakeProductPrice>> GetMakeProductPriceReport(string FromDate, string ToDate, PaginatedQueryModel query);
        Task<List<spCommodityReceiptReports>> GetCommodityReceiptReportsRial(string FromDate, string ToDate, int? CommodityId, int? AccountHeadId, int? WarehouseId, int? DocumentNo, PaginatedQueryModel query);
        Task<List<spContradictionDebit>> GetContradictionDebit(string FromDate, string ToDate, int? CommodityId, string AccountHeadId, PaginatedQueryModel query);
    }
}
