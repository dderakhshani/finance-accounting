using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public interface IWarehouseLayoutQueries : IQuery
    {
        Task<WarehouseLayoutGetIdModel> GetById(int id);
        Task<PagedList<WarehouseLayoutModel>> GetParentIdAllChildByCapacityAvailable(int id);
        Task<PagedList<WarehouseLayoutModel>> GetAll(PaginatedQueryModel query);
        Task<PagedList<WarehouseLayoutModel>> GetTreeAll(PaginatedQueryModel query);
        Task<PagedList<WarehouseLayoutsCommoditiesQuantityModel>> GetWarehouseLayoutCommodityId(PaginatedQueryModel query);
        Task<PagedList<WarehouseLayoutModel>> GetSuggestionWarehouseLayoutByCommodityCategories(PaginatedQueryModel query, int commodityId);
        Task<PagedList<StocksCommoditiesModel>> GetWarehouseLayoutHistoryCommodityId(string FromDate, string ToDate, PaginatedQueryModel query);
        Task<PagedList<WarehouseHistoriesDocumentViewModel>> GetAllHistoriesDocument(string FromDate, string ToDate, PaginatedQueryModel query);
        Task<PagedList<spGetWarehouseLayoutQuantities>> GetWarehouseLayoutQuantities(int? warehouseId, int? CommodityId, PaginatedQueryModel query);
        Task<PagedList<WarehouseLayoutsCommoditiesViewArani>> GetLastChangeWarehouseLayoutQuantities(double? hour);
        Task<PagedList<WarehouseLayoutModel>> GetAllByParentId(int warehouseId, int? parentId, PaginatedQueryModel query);
    }
}

