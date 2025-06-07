using Eefa.Common.Data;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data.Query;
using Eefa.Common;

namespace Eefa.Commodity.Application.Queries.Bom
{
    public interface IBomQueries : IQuery
    {
        Task<ServiceResult<BomModel>> GetBomById(int id);
        Task<ServiceResult<PagedList<BomModel>>> GetBoms(PaginatedQueryModel query);
        Task<ServiceResult<PagedList<BomValueHeaderModel>>> GetBomValueHeadersByCommodityId(int commodityId, PaginatedQueryModel query);
        Task<ServiceResult<BomValueHeaderModel>> GetBomValueHeaderById(int id);


        Task<ServiceResult<PagedList<BomModel>>> GetBomsByCommodityCategoryId(PaginatedQueryModel query);

        Task<ServiceResult<PagedList<BomItemModel>>> GetBomItemsByBomId(int bomId, PaginatedQueryModel query);


        Task<PagedList<BomValueHeaderModel>> GetBomValueHeadersByBomId(int bomId, PaginatedQueryModel query);

        Task<PagedList<BomValueModel>> GetBomValuesByBomValueHeaderId(int bomValueHeaderId, PaginatedQueryModel query);
        Task<ServiceResult<PagedList<BomValueHeaderModel>>> GetAllBomValueHeaders(PaginatedQueryModel query);

        Task<BomValueModel> GetBomValueById(int id);


    }
}

