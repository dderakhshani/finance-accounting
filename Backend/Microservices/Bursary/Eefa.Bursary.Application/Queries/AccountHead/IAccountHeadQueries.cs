using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Bursary.Application.Queries.AccountHead
{
    public interface IAccountHeadQueries : IQuery
    {
        Task<PagedList<AccountHeadViewModel>> GetAll(PaginatedQueryModel query);


    }
}
