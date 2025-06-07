using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public interface IProcedureCallService : IQuery
    {
        //---------------------میانگین قیمت کالا ------------------------------------

        Task<SpDocumentItemsPriceBuy> GetAndUpdatePriceBuy(int CommodityId, int WarehouseId, int? DocumentItemsId = null);
        
        //---------------------محاسبه تعداد ورودی های هر کالا بر اساس درخواست ثبت شده--------
        Task<SpCalculateRemainQuantityRequest> CalculateRemainQuantityRequest(string RequestNo, int CommodityId);
        Task<SpCalculateRemainQuantityRequest> CalculateRemainQuantityRequestCommodityByPerson(string RequestNo, int CommodityId, int RequestDocumentItemId);

        Task ValidationIssuedDocuments();
        Task<PagedList<SpReceiptGroupbyInvoice>> GetAllReceiptGroupInvoice(DateTime? FromDate,
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
        Task<List<SpTejaratReportImportCommodity>> TejaratReports(string documentDate, int DocumentStauseBaseValue);
        Task<List<WarehousesLastLevelViewModel>> GetWarehousesLastLevelByCodeVoucherGroupId(int CodeVoucherGroupId);
        Task<List<SpGetAudit>> GetAuditById(int Id, string TableName);
        Task<List<spGetConsumptionCommodityByRequesterReferenceId>> GetConsumptionCommodityByRequesterReferenceId(int RequesterReferenceId, int CommodityId, string CodeVoucherGroup);
        Task<List<spGetMonthlyEntryToWarehouse>> GetMonthlyEntryToWarehouse(int? CommodityId, int? WarehouseId, int? YearId, int? EnterMode, int? UserId);
        //گزارش تعداد موجودی و ارز ریالی کالا ها در انبارها 
        Task<List<spCommodityReports>> GetCommodityReports(DateTime FromDate, DateTime ToDate, int? CommodityId, string WarehouseId, int? UserId, int? YearId, string CommodityTitle, PaginatedQueryModel query);
        Task<List<spCommodityReportsRial>> GetCommodityReportsRial(DateTime FromDate, DateTime ToDate, int? CommodityId, string AccountHeadId, int? UserId, int? YearId, PaginatedQueryModel query);
        //-----------گزارش رسیدهای ورود و خروج مربوط به یک کالا
        Task<List<spCommodityReceiptReports>> GetCommodityReceiptReports(DateTime FromDate, DateTime ToDate, int? CommodityId, int? WarehouseId, int? UserId, int? YearId, int? DocumentNo, int? RequestNo,string CommodityTitle, PaginatedQueryModel query);
        Task<List<spCommodityReceiptReports>>  GetCommodityReceiptReportsRial(spCommodityReceiptReportsRialParam model,  PaginatedQueryModel query);
        Task<List<spGetLogTable>> spGetLogTable(int Id, string TableName, string ColumnName, string ShemaName);
        Task<int> UpdateStockQuantity(int? CommodityId, int? WarehouseId);
        Task<int> UpdateWarehouseLayoutQuantities(int? CommodityId, int? WarehouseLayoutId);
        Task<List<spGetDocumentItemForTadbir>> GetDocumentItemForTadbir(DateTime FromDate, DateTime ToDate, int? CodeVoucherGroupId, int? DocumentNo, int? WarehouseId, int? UserId);
        Task<List<spReportControlsWarehouseLayoutQuantitiesByTadbir>> GetReportControlsWarehouseLayoutQuantitiesByTadbir(int warehouseId, int YearId);
        Task DeleteInventory_StockFromTadbir(int warehouseId);
        Task<object> UpdateWarehouseCommodityPrice(int warehouseId, int YearId);
        Task<spUpdateWarehouseCommodityAvailabe2> UpdateWarehouseCommodityAvailable(int warehouseId, int? YearId, int UserId);
        //گزارش ورودی های خرید در یک صورتحساب
        Task<List<spReceiptInvoice>> GetAllReceiptInvoice(DateTime? FromDate, DateTime? ToDate, string InvoiceNo, int? CreditAccountReferenceId, int PageNumber, int PageRow, PaginatedQueryModel query);

        Task<List<spGetCommodity>> GetCommodity(int? warehouseId, string searchTerm, bool? IsConsumable, bool? IsAsset, PaginatedQueryModel query);
        Task<Stp_Acc_Foreign_orders> GetCostImportReceipts(int CompanyId, int YearId, int ReferenceId);
        Task<PagedList<spGetWarehouseLayoutQuantities>> GetWarehouseLayoutQuantities(int? warehouseId, int? CommodityId, int? UserId, PaginatedQueryModel query);
        //کرایه حمل با سیستم دانا
        Task<PagedList<Stp_FreightPays>> GetFreightPays(DateTime FromDate, DateTime ToDate, int? AccountReferenceId, PaginatedQueryModel query);
        //وزن محصول تولیدی
        Task<IQueryable<MakeProductPriceReportModel>> spGetTotalWeightProduct(DateTime? FromDate, DateTime? ToDate, PaginatedQueryModel query);

        Task<List<spCommodityReportsSumAll>> GetCommodityReportsSumAll(DateTime FromDate, DateTime ToDate, int? CommodityId, string WarehouseId, int? UserId, int? YearId, string CommodityTitle, PaginatedQueryModel query);

        Task<PagedList<spCommodityCost>> spGetCommodityCost(DateTime? FromDate, DateTime? ToDate, string WarehouseId, PaginatedQueryModel query);
         Task<List<spContradictionDebit>> GetContradictionDebit(DateTime FromDate, DateTime ToDate, int? CommodityId, string AccountHeadId, int? UserId, int? YearId, PaginatedQueryModel query);
        Task<PagedList<spCommodityReportsWithWarehouse>> GetCommodityReportsWithWarehouse(DateTime? FromDate,
            DateTime? ToDate,
             string WarehouseId,
             string CodeVoucherGroupId,
             int DocumentStauseBaseValue,
             int? UserId,
             int? yearId,
            PaginatedQueryModel query);
        Task<int> UpdateAllStockQuantity();
        Task<int> WarehouseCheckLoseData();
        Task<int> UpdateWarehouseLayout(int WarehouseId, int UserId);
        Task<int> RemoveCommodityFromWarehouse(int WarehouseId, int UserId);
        Task<int> UpdateAddNewCommodity(int UserId);
        Task<int> ArchiveDocumentHeadsByDocumentDate(DateTime? FromDate, DateTime? ToDate, int WarehouseId, int UserId, int DocumentStatuesBaseValue);
        Task<int> WarehouseRequestExitUpdateIsDeleted();
        Task<int> UpdateVoucherHeadAfterRepairCardex();
        Task<int> UpdateWarehouseCardex(spUpdateWarehouseCardexParam model);
        Task<int> ComputeAvgPrice(spComputeAvgPriceParam model1, spUpdateinventory_CaredxRepairParam model2);
        Task<int> InsertDocumentHeads(WarehouseIOCommodity model);
        Task<PagedList<spErpDarkhastJoinDocumentHeads>> GetErpDarkhastJoinDocumentHeads(spErpDarkhastJoinDocumentHeadsParam model, PaginatedQueryModel query);
        Task<Receipt> InsertDocumentHeadsForIOCommodity(InsertDocumentHeadsForIOCommodity model);
        Task<Receipt> UpdateDocumentHeadsForIOCommodity(InsertDocumentHeadsForIOCommodity model);
        Task<Receipt> UpdateDocumentHeadsForIOCommodityMaterial(InsertDocumentHeadsForIOCommodity model);
    }

}

