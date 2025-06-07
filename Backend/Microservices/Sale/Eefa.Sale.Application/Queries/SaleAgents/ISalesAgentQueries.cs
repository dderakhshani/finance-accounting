using Eefa.Common.Data.Query;
using Eefa.Common.Data;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.CommandQuery;

namespace Eefa.Sale.Application.Queries.SaleAgents
{
    public interface ISalesAgentQueries : IQuery
    {
        Task<ServiceResult<PagedList<SalesAgentModel>>> GetAll(PaginatedQueryModel query);
        Task<ServiceResult<SalesAgentModel>> GetById(int id);


    }
}