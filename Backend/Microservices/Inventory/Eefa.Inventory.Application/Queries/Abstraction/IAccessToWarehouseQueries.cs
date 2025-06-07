using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{
    public interface IAccessToWarehouseQueries : IQuery
    {
        Task<PagedList<SpGetAudit>> GetAll(PaginatedQueryModel query);
        Task<PagedList<SpGetAudit>> GetById(int Id);
        Task<List<int>> GetUserId(int UserId, string TableName);
        Task<PagedList<UserModel>> GetUsers(PaginatedQueryModel query);
    }
}
