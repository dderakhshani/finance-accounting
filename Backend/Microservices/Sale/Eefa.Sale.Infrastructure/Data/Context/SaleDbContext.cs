
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Sale.Infrastructure.Data.Context.Configurations;
using Eefa.Sale.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
namespace Eefa.Sale.Infrastructure.Data.Context;

public partial class SaleDbContext : AuditableDbContext, ISaleDbContext
{
    public SaleDbContext(DbContextOptions<SaleDbContext> options, ICurrentUserAccessor currentUserAccessor)
        : base(options,currentUserAccessor)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<FixedPriceHistory> FixedPriceHistories { get; set; }

    public virtual DbSet<SalePriceList> SalePriceLists { get; set; }

    public virtual DbSet<SalePriceListDetail> SalePriceListDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);

        base.OnModelCreating(modelBuilder);

     
       
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.LogTo(Console.WriteLine);

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
