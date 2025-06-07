using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Bursary.Application.Queries.AccountReferenceGroup
{
   public interface IAccountReferencesGroupQueries:IQuery
    {
        Task<PagedList<AccountReferencesGroupViewModel>> GetAll(PaginatedQueryModel query);
        Task<PagedList<AccountReferencesGroupViewModel>> GetReferenceGroupsBy(int id);

    }


}
