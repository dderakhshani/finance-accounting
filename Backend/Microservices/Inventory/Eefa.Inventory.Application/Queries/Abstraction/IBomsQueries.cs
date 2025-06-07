using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Application.Models;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;

namespace Eefa.Inventory.Application
{
    public interface IBomsQueries : IQuery
    {
        Task<PagedList<CommodityBomsModel>> GetAll(string FromDate,string ToDate, PaginatedQueryModel query);
            
        Task<PagedList<CommodityBomsModel>> GetByCommodityId(int CommodityId);
        Task<CommodityBomsModel> GetById(int id);
    }
}
