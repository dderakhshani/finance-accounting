using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public interface IAuditQueries : IQuery
    {
        Task<PagedList<SpGetAudit>> GetAll(string FromDate,string ToDate, PaginatedQueryModel query);


        Task<PagedList<spGetLogTable>> GetAuditById(int PrimaryId, string TableName);
    }
}
