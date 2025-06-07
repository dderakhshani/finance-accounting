using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Purchase.Application.Models;

namespace Eefa.Purchase.Application.Queries.Abstraction
{
    public interface IBaseValueQueries : IQuery
    {
        Task<PagedList<BaseValueModel>> GetCurrencyBaseValue();
        Task<List<string>> GetDocumentTagBaseValue();
        Task<PagedList<BaseValueModel>> GetInvoiceALLBaseValue();

        Task<string> GeVatDutiesTaxValue();
    }
}
