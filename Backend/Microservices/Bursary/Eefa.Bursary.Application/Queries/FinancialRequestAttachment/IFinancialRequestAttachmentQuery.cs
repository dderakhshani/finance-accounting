using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Queries.FinancialRequestAttachment
{
    public interface IFinancialRequestAttachmentQuery : IQuery
    {
        Task<PagedList<FinancialAttachmentModel>> GetAll(PaginatedQueryModel query);
    }
}
