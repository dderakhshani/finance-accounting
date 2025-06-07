using System;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public interface IAraniRequstCommandQueries : IQuery
    {

        Task<ReceiptQueryModel> GetPurchaseRequestById(int requestId);
        Task<RequestCommodityWarehouseModel> leavingWarehouseRequestById(int requestId, int warehouseId);
        Task<ReceiptQueryModel> GetReturnCommodityWarehouse(int requestId);
        Task<PagedList<spErpDarkhastJoinDocumentHeads>> GetErpDarkhastJoinDocumentHeads(
           DateTime? FromDate,
           DateTime? ToDate,
           string RequestNo,
           PaginatedQueryModel query);

    }
}

