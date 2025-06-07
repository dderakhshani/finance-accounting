using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Inventory.Application
{
    public interface IWarehousQueries : IQuery
    {
        Task<WarehouseModel> GetById(int id);
        Task<PagedList<WarehouseModel>> GetAll(PaginatedQueryModel query);

        Task<PagedList<WarehousesLastLevelViewModel>> GetWarehousesLastLevel(PaginatedQueryModel paginatedQuery);
        Task<PagedList<WarehousesLastLevelViewModel>> GetWarehousesLastLevelByCodeVoucherGroupId(int CodeVoucherGroupId, PaginatedQueryModel paginatedQuery);
    }
}

