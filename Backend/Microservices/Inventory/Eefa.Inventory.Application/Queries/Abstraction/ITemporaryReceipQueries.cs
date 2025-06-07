using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data.Query;
using Eefa.Common.Data;

namespace Eefa.Inventory.Application
{
    public interface ITemporaryReceipQueries : IQuery
    {
        Task<ReceiptQueryModel> GetById(int id);
        Task<ReceiptQueryModel> GetByDocumentNo(int DocumentNo,int warehouseId);
        Task<ReceiptQueryModel> GetByDocumentNo(int DocumentNo);
        Task<ReceiptQueryModel> GetByRequesterReferenceId(int RequesterReferenceId, string FromDate, string ToDate);
        Task<PagedList<ReceiptQueryModel>> GetAll(int? DocumentStatuesBaseValue, string FromDate, string ToDate, PaginatedQueryModel query);


    }
}

