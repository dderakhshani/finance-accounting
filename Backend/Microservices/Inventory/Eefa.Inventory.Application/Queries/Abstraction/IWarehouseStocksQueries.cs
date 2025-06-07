using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Inventory.Application
{
    public interface IWarehouseStocksQueries : IQuery
    {
        Task<bool> GetIsAvailableCommodity(int? CommodityId, string CommodityCode);
        Task<PagedList<WarehouseStockModel>> GetAll(PaginatedQueryModel query);
        Task<string> Url(int Id);
    }
}
