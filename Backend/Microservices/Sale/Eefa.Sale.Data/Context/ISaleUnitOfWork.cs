
using Eefa.Sale.Data.Entities;
using Eefa.Common.Data;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Sale.Data.Context
{
    public interface ISaleUnitOfWork : IUnitOfWork
    {
        public  DbSet<Customer> Customers { get; set; }

        public  DbSet<FixedPriceHistory> FixedPriceHistories { get; set; }

        public  DbSet<SalePriceList> SalePriceLists { get; set; }

        public  DbSet<SalePriceListDetail> SalePriceListDetails { get; set; }


    }

}