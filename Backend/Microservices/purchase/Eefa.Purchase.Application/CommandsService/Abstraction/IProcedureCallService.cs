using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Purchase.Application.Models;

namespace Eefa.Purchase.Application
{
    public interface IProcedureCallService : IQuery
    {
        //---------------------میانگین قیمت کالاهای خروجی------------------------------------
         Task<SpDocumentItemsPriceBuy> GetPriceBuy(int CommodityId);
        Task ModifyDocumentAttachments(List<int> attachmentIds, int DocumentHeadId);

    }
}

