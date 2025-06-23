
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Warehouse.Infrastructure.Data.Context.Configurations;
using Eefa.Warehouse.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace Eefa.Warehouse.Infrastructure.Data.Context;

public partial class WarehouseDbContext : AuditableDbContext, IWarehouseDbContext
{
    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options, ICurrentUserAccessor currentUserAccessor)
        : base(options, currentUserAccessor)
    {
    }

    public virtual DbSet<Warehous> Warehouses { get; set; }

    public virtual DbSet<WarehouseLayout> WarehouseLayouts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.WarehousConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.WarehouseLayoutConfiguration());

        modelBuilder.HasSequence("SeqPayment", "bursary");
        modelBuilder.HasSequence("SeqReceive", "bursary");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
