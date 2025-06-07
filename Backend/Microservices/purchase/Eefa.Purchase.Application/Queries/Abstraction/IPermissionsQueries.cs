using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Purchase.Application.Models;

namespace Eefa.Purchase.Application.Queries.Abstraction
{ 
    public interface IPermissionsQueries : IQuery
    {
        Task<PermissionsModel> GetById(int id);
        Task<PagedList<PermissionsModel>> GetAll(PaginatedQueryModel query);


    }
}
