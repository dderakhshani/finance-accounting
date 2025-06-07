using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;

namespace Eefa.Inventory.Application
{
    public interface IPersonsDebitedCommoditiesQueries : IQuery
    {
        Task<PagedList<PersonsDebitedCommoditiesModel>> GetAll(string FromDate,
            string ToDate, PaginatedQueryModel query);
        Task<PagedList<PersonsDebitedCommoditiesModel>> GetByDocumentId(int DocumentItemId, int CommodityId, PaginatedQueryModel query);
        Task<PersonsDebitedCommoditiesModel> GetById(int id);
    }
}
