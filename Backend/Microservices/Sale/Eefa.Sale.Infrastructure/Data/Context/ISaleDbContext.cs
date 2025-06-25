using Eefa.Common.Data;
using Eefa.Sale.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Sale.Infrastructure.Data.Context
{
    public interface ISaleDbContext : IUnitOfWork
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<FixedPriceHistory> FixedPriceHistories { get; set; }

        public DbSet<SalePriceList> SalePriceLists { get; set; }

        public DbSet<SalePriceListDetail> SalePriceListDetails { get; set; }
    }
}