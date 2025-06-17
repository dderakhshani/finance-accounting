
using Eefa.Commodity.Data.Context.Configurations;
using Eefa.Commodity.Data.Entities;
using Eefa.Common;
using Eefa.Common.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace Eefa.Commodity.Data.Context;

public partial class CommodityUnitOfWork : AuditableDbContext, ICommodityUnitOfWork
{
    public CommodityUnitOfWork(DbContextOptions<CommodityUnitOfWork> options, ICurrentUserAccessor currentUserAccessor)
        : base(options,currentUserAccessor)
    {
    }

    public virtual DbSet<BaseValue> BaseValues { get; set; }

    public virtual DbSet<BaseValueType> BaseValueTypes { get; set; }

    public virtual DbSet<Bom> Boms { get; set; }

    public virtual DbSet<BomItem> BomItems { get; set; }

    public virtual DbSet<BomValue> BomValues { get; set; }

    public virtual DbSet<BomValueHeader> BomValueHeaders { get; set; }

    public virtual DbSet<BomsView> BomsViews { get; set; }

    public virtual DbSet<CategoryPropertyMapping> CategoryPropertyMappings { get; set; }

    public virtual DbSet<Entities.Commodity> Commodities { get; set; }

    public virtual DbSet<CommodityCategory> CommodityCategories { get; set; }

    public virtual DbSet<CommodityCategoryProperty> CommodityCategoryProperties { get; set; }

    public virtual DbSet<CommodityCategoryPropertyItem> CommodityCategoryPropertyItems { get; set; }

    public virtual DbSet<CommodityPropertyValue> CommodityPropertyValues { get; set; }

    public virtual DbSet<DocumentItem> DocumentItems { get; set; }

    public virtual DbSet<MeasureUnit> MeasureUnits { get; set; }

    public virtual DbSet<MeasureUnitConversion> MeasureUnitConversions { get; set; }
    public virtual DbSet<CommoditeisView> CommoditeisView { get; set; } = default!;
    public virtual DbSet<BomsView> BomsView { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.BaseValueConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.BaseValueTypeConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.BomConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.BomItemConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.BomValueConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.BomValueHeaderConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.BomsViewConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CategoryPropertyMappingConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CommodityConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CommodityCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CommodityCategoryPropertyConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CommodityCategoryPropertyItemConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CommodityPropertyValueConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.DocumentItemConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.MeasureUnitConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.MeasureUnitConversionConfiguration());

        modelBuilder.HasSequence("SeqPayment", "bursary");
        modelBuilder.HasSequence("SeqReceive", "bursary");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
