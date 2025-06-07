using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;

namespace Eefa.Inventory.Application
{
    public interface IBaseValueQueries : IQuery
    {
        Task<PagedList<BaseValueModel>> GetCurrencyBaseValue();
        Task<List<string>> GetDocumentTagBaseValue();
        Task<PagedList<BaseValueModel>> GetReceiptALLBaseValue();
        Task<PagedList<BaseValueModel>> GetDepreciationTypeBaseValue();
        Task<PagedList<BaseValueModel>> GetCommodityGroupBaseValue();

    }
}
