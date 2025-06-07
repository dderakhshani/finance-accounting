using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Threading.Tasks;

namespace Eefa.Sale.Application.Queries.Customers
{
    public interface ICustomerQueries : IQuery
    {
        Task<ServiceResult<PagedList<CustomerModel>>> GetAll(PaginatedQueryModel query);
        Task<ServiceResult<CustomerModel>> GetById(int id);


    }
}
