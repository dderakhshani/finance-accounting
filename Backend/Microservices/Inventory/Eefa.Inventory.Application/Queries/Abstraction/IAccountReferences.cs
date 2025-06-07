using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public interface IAccountReferences:IQuery
    {

        Task<PagedList<AccountReferenceViewModel>> GetAccountReferences(PaginatedQueryModel query, int? AccountHeadId);
        Task<PagedList<AccountReferenceModel>> GetRequesterReferenceWarhouse(int DocumentStauseBaseValue, string FromDate, string ToDate, PaginatedQueryModel query);
        Task<PagedList<AccountReferenceModel>> GetReferenceReceipt(PaginatedQueryModel query);
        Task<PagedList<AccountReferenceViewModel>> GetAccountReferencesPerson(PaginatedQueryModel query);
        Task<PagedList<AccountReferenceViewModel>> GetAccountReferencesProvider(PaginatedQueryModel query);
        Task<PagedList<AccountHead>> GetAccountHead(PaginatedQueryModel query);

    }
}
