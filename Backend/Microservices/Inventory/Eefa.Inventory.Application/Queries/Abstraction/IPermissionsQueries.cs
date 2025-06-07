using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Eefa.Inventory.Application
{
    public interface IPermissionsQueries : IQuery
    {
        Task<PermissionsModel> GetById(int id);
        Task<PagedList<PermissionsModel>> GetAll(PaginatedQueryModel query);
        IQueryable<User> GetAllUserByPermision(int permisionId);


    }
}
