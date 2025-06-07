using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;


namespace Eefa.Logistic.Application
{
    public interface IProcedureCallService : IQuery
    {
     
        Task ModifyDocumentAttachments(List<int> attachmentIds, int DocumentHeadId);

    }
}

