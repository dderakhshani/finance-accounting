using Eefa.Common.CommandQuery;
using Eefa.Sale.Infrastructure.Data.Entities;

namespace Eefa.Sale.Application.Queries.PriceListDetails
{
    public interface IPriceListDetailsQueries : IQuery
    {
        Task<List<SalePriceListDetail>> GetAll();
    }
}