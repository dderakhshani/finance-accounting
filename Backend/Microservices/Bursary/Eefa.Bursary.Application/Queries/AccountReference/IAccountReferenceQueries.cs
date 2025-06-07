using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Bursary.Application.Queries.AccountReference
{
   public interface IAccountReferenceQueries : IQuery
    {
        Task<AccountReferenceModel> GetById(int id);
        Task<PagedList<AccountReferenceModel>> GetAll(PaginatedQueryModel query);

        Task<PagedList<AccountReferenceModel>> ReferenceAccountsByGroupId(int id);

    }
}
