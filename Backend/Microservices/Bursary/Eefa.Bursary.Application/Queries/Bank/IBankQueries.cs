using System.Threading.Tasks;
using Eefa.Common.CommandQuery;

using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Bursary.Application.Queries.Bank
{
    public interface IBankQueries:IQuery
    {
   
        Task<PagedList<BankModel>> GetAll(PaginatedQueryModel query);

    }
}
