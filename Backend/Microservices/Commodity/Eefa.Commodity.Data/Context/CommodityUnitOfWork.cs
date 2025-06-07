using Microsoft.EntityFrameworkCore;
using System;
using Eefa.Commodity.Data.Entities;
using Eefa.Common.Data;
using Eefa.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eefa.Commodity.Data.Context
{
    public partial class CommodityUnitOfWork : AuditableDbContext, ICommodityUnitOfWork
    {
        public virtual DbSet<BaseValue> BaseValues { get; set; } = default!;
        public virtual DbSet<BaseValueType> BaseValueTypes { get; set; } = default!;
        public virtual DbSet<Bom> Boms { get; set; } = default!;
        public virtual DbSet<BomItem> BomItems { get; set; } = default!;
        public virtual DbSet<BomValue> BomValues { get; set; } = default!;
        public virtual DbSet<BomValueHeader> BomValueHeaders { get; set; } = default!;
        public virtual DbSet<CategoryPropertyMapping> CategoryPropertyMappings { get; set; } = default!;
        public virtual DbSet<Data.Entities.Commodity> Commodities { get; set; } = default!;
        public virtual DbSet<CommodityCategory> CommodityCategories { get; set; } = default!;
        public virtual DbSet<CommodityCategoryProperty> CommodityCategoryProperties { get; set; } = default!;
        public virtual DbSet<CommodityCategoryPropertyItem> CommodityCategoryPropertyItems { get; set; } = default!;
        public virtual DbSet<CommodityPropertyValue> CommodityPropertyValues { get; set; } = default!;
        public virtual DbSet<MeasureUnit> MeasureUnits { get; set; } = default!;
        public virtual DbSet<BomsView> BomsView { get; set; } = default!;
        public virtual DbSet<MeasureUnitConversion> MeasureUnitConversions { get; set; } = default!;
        public virtual DbSet<DocumentItem> DocumentItems { get; set; } = default!;
        public virtual DbSet<CommoditeisView> CommoditeisView { get; set; } = default!;
        
        public CommodityUnitOfWork(DbContextOptions<CommodityUnitOfWork> options, ICurrentUserAccessor currentUserAccessor)
           : base(options, currentUserAccessor)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
            //--------------------------Convert Date To UTC-----------------------------
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsKeyless)
                {
                    continue;
                }

                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
