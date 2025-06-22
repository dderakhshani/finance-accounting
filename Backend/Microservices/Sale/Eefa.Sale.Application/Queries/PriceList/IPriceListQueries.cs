using Eefa.Common.CommandQuery;
using Eefa.Sale.Infrastructure.Data.Entities;

namespace Eefa.Sale.Application.Queries.PriceList
{
    public interface IPriceListQueries:IQuery
    {
        Task<List<SalePriceList>> GetAll();
        Task<SalePriceList> GetById(int id);
    }
}