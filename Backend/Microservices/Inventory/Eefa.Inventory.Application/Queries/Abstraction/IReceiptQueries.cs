using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public interface IReceiptQueries : IQuery
    {
        Task<ReceiptQueryModel> GetById(int id);
       
        Task<ReceiptQueryModel> GetPlacementById(int id, int? WarehouseId);
        Task<PagedList<ReceiptQueryModel>> GetAll(int? DocumentStatuesBaseValue, bool? IsImportPurchase, string FromDate, string ToDate, PaginatedQueryModel query);
        Task<ReceiptQueryModel> GetByListId(List<int> ListId);
        Task<PagedList<ReceiptItemModel>> GetALLDocumentItemsBom(int DocumentItemsId);
        Task<ReceiptItemModel> GetDocumentItemsBomById(int Id);
        Task<PagedList<ReceiptQueryModel>> GetByViewId(int ViewId, string FromDate, string ToDate, PaginatedQueryModel query);
        Task<PagedList<ReceiptQueryModel>> GetAllReceiptItemsCommodity(int? DocumentStatuesBaseValue, string FromDate, string ToDate, PaginatedQueryModel query);
        Task<ReceiptQueryModel> GetByDocumentNoAndCodeVoucherGroupId(int DocumentNo, int codeVoucherGroupId);
        Task<ReceiptQueryModel> GetByDocumentNoAndDocumentStauseBaseValue(int DocumentNo, int DocumentStauseBaseValue);
        
        Task<PagedList<ReceiptQueryModel>> GetByDocumentStatuesBaseValue(int DocumentStatuesBaseValue, string FromDate, string ToDate, PaginatedQueryModel query);
        Task<PagedList<ReceiptQueryModel>> GetAllAmountReceipt(string FromDate, string ToDate, PaginatedQueryModel query);
        
        
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
       
        Task<ReceiptQueryModel> GetByInvoiceNo(string InvoiceNo, string Date, int? CreditAccountReferenceId);
        int GetNewCodeVoucherGroupId(int CodeVoucherGroupId, int DocumentStatuesBaseValue);
        Task<int[]> GetDocumentAttachmentIdByDocumentIdId(int DocumentId);
        Task<ReceiptQueryModel> GetByDocumentId(int DocumentId);
        
        Task<CorrectionRequest> GetCorrectionRequestById(int DocumentId, int status);
        Task<CorrectionRequest> GetCorrectionRequestById(int Id);
        Task<PagedList<ReceiptQueryModel>> GetComprehensiveList(string FromDate, string ToDate, PaginatedQueryModel query);
        Task<PagedList<WarehouseHistoriesDocumentViewModel>> GetAllReceiptItems(string FromDate, string ToDate, PaginatedQueryModel query);
        Task<long> GetCostImportReceipts(int ReferenceId);
        Task<DocumentItem> GetStartReceiptsItem(int CommodityId, int WarehouseId);
        Task<ReceiptQueryModel> GetByDocumentIdByVoucherHeadId(int DocumentHeadId);
        Task<PagedList<WarehouseRequestExitViewModel>> GetWarehouseRequestExitView(string FromDate, string ToDate, PaginatedQueryModel query);
        Task<PagedList<View_Sina_FinancialOperationNumberModel>> GeView_Sina_FinancialOperationNumber(PaginatedQueryModel query);
    }
}

