using System.Threading.Tasks;
using Eefa.Bursary.Application.Models;
using Eefa.Common.CommandQuery;

using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Bursary.Application.Queries.ChequeSheet
{
    public interface IChequeSheetQueries:IQuery
    {
       
        Task<PagedList<ChequeSheetModel>> GetAll(PaginatedQueryModel query);
        Task<PagedList<ChequeSheetModel>> GetUsedCheques(PaginatedQueryModel query);
        Task<PagedList<ChequeSheetModel>> GetChequeSheetById(int chequeId);




    }
}
