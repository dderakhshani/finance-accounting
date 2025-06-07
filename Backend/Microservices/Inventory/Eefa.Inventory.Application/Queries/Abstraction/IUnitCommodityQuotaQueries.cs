using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Inventory.Application
{

    public interface IUnitCommodityQuotaQueries : IQuery
    {
        Task<UnitCommodityQuotaModel> GetById(int id);
        Task<PagedList<UnitCommodityQuotaModel>> GetAll(PaginatedQueryModel query);
        Task<PagedList<UnitsModel>> GetAllUnits(PaginatedQueryModel query);
        Task<PagedList<QuotaGroupModel>> GetAllQuotaGroup(PaginatedQueryModel query);

    }
}

