 
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;

namespace Eefa.Bursary.Application.Queries.DocumentHead
{
   public interface IDocumentHeadQueries : IQuery
    {
        Task<PagedList<DocumentHeadModel>> GetDocumentHeadsByReferenceId(PaginatedQueryModel query);
    }
}
