using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public interface IWarehouseCountFormHeadQueries : IQuery
    {
        Task<PagedList<WarehouseCountFormHeadModel>> GetAll(PaginatedQueryModel query);
        Task<PagedList<WarehouseCountFormDetailsModel>> GetDetailsByHeadId(PaginatedQueryModel query, int warehouseCountFormHeadId);
        Task<PagedList<WarehouseCountFormDetailsModel>> GetAlldiscrepancies(PaginatedQueryModel query, int warehouseCountFormHeadeId);
        Task<WarehouseCountFormHeadModel> GetWarehouseCountFormHeadById(int id);
        Task<List<WarehouseCountFormHeadModel>> GetWarehouseCountFormHeadByParentId(int parentId);
        Task<PagedList<WarehouseCountFormReport>> GetWarehouseCountReport(PaginatedQueryModel query, int warehouseCountFormHeadeId);
        Task<PagedList<WarehouseCommodityWithPriceModel>> GetCommoditisWithPrice(PaginatedQueryModel query, int warehouseId);
    }
}

