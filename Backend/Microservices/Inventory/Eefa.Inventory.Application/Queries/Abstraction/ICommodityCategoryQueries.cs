using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Application.Models.CommodityCategory;

namespace Eefa.Inventory.Application { 
   public interface ICommodityCategoryQueries : IQuery
    {
       
        Task<PagedList<CommodityCategoryPropertyModel>> GetPropertyByWarehouseLayoutId(int GetPropertyByCategoryId, PaginatedQueryModel query);
        Task<PagedList<CommodityCategoryPropertyItemModel>> GetPropertyItemsByWarehouseLayoutId(int warehouseLayoutId, PaginatedQueryModel query);
        Task<PagedList<CommodityCategoryModel>> GetCategores(int? ParentId, int WarehouseId, PaginatedQueryModel query);
        Task<PagedList<CommodityCategoryModel>> GetCategoresCodeAssetGroup(PaginatedQueryModel query);
        Task<PagedList<CommodityCategoryModel>> GetAll(PaginatedQueryModel paginatedQuery);
        Task<PagedList<CommodityCategoryModel>> GetTreeAll(PaginatedQueryModel query);
       
    }
}
