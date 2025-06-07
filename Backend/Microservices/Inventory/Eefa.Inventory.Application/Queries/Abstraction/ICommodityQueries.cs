using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Inventory.Application
{
    public interface ICommodityQueries : IQuery
    {


        Task<PagedList<ViewCommodityModel>> GetCommodity(int? warehouseId, bool? isOnlyFilterByWarehouse, string searchTerm, PaginatedQueryModel query);

        Task<PagedList<ViewCommodityModel>> GetCommodityById(int Id, PaginatedQueryModel query);
        
        double GetQuantityCommodity(int warehouseId, int CommodityId);
    }
}
