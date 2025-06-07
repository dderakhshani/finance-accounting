using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Queries.Commodity
{
    public interface ICommodityQueries : IQuery
    {
        Task<ServiceResult<CommodityModel>> GetById(int id);
        Task<ServiceResult<PagedList<CommodityModel>>> GetAll(int CommodityCategoryId,PaginatedQueryModel query);

        Task<ServiceResult<CommodityCategoryModel>> GetCategoryById(int id);
        Task<ServiceResult<PagedList<CommodityCategoryModel>>> GetAllCategories(PaginatedQueryModel query);
        Task<ServiceResult<List<CommodityCategoryModel>>> GetCategoryParentTree(string levelCode);

        Task<ServiceResult<CommodityCategoryPropertyModel>> GetCategoryPropertyById(int id);
        Task<ServiceResult<PagedList<CommodityCategoryPropertyModel>>> GetAllCategoryProperties(PaginatedQueryModel query);

        Task<ServiceResult<CommodityCategoryPropertyItemModel>> GetCategoryPropertyItemById(int id);
        Task<ServiceResult<PagedList<CommodityCategoryPropertyItemModel>>> GetAllCategoryPropertyItems(PaginatedQueryModel query);

        Task<ServiceResult<CommodityPropertyValueModel>> GetPropertyValueById(int id);
        Task<ServiceResult<PagedList<CommodityPropertyValueModel>>> GetAllPropertyValues(PaginatedQueryModel query);

    }
}

