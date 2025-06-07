using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{
    public interface IDocumentHeadExtraCostQueries : IQuery
    {
        Task<PagedList<DocumentHeadExtraCostModel>> GetAll(PaginatedQueryModel query);
            
        Task<PagedList<DocumentHeadExtraCostModel>> GetByDocumentHeadId(List<int> documentHeadIds);
        Task<DocumentHeadExtraCostModel> GetById(int id);
        Task<decimal> GetTotalExtraCost(List<int> documentHeadIds);
    }
}
