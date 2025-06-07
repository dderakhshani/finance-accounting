using System.Threading.Tasks;
using Eefa.Common.CommandQuery;

using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Bursary.Application.Queries.Cheque
{
    public interface IChequeQueries:IQuery
    {
        Task<ChequeModel> GetById(int id);
        Task<PagedList<ChequeModel>> GetAll(PaginatedQueryModel query);

    }
}
