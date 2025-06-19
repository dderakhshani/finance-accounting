
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Sale.Data.Context;
using Eefa.Sale.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace Eefa.Sale.Data.Context;

public partial class SaleUnitOfWork : AuditableDbContext, ISaleUnitOfWork
{
    public SaleUnitOfWork(DbContextOptions<SaleUnitOfWork> options, ICurrentUserAccessor currentUserAccessor)
        : base(options,currentUserAccessor)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<FixedPriceHistory> FixedPriceHistories { get; set; }

    public virtual DbSet<SalePriceList> SalePriceLists { get; set; }

    public virtual DbSet<SalePriceListDetail> SalePriceListDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.FixedPriceHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.SalePriceListConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.SalePriceListDetailConfiguration());

        modelBuilder.HasSequence("SeqPayment", "bursary");
        modelBuilder.HasSequence("SeqReceive", "bursary");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
