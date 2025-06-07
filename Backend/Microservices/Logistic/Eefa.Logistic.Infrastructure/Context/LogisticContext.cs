using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Logistic.Domain;
using Eefa.Logistic.Infrastructure.Context.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Eefa.Logistic.Infrastructure.Context
{
    public partial class LogisticContext : AuditableDbContext
    {
        public virtual DbSet<MapSamatozinToDana> MapSamatozinToDana { get; set; } = default!;
        public virtual DbSet<AccountReferenceView> AccountReferenceView { get; set; } = default!;
        public virtual DbSet<ReceiptWithCommodityView> ReceiptWithCommodityView { get; set; } = default!;

        public LogisticContext(DbContextOptions<LogisticContext> options, ICurrentUserAccessor currentUserAccessor)
           : base(options, currentUserAccessor)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            //--------------------------Table---------------------------------------
            modelBuilder.ApplyConfiguration(new MapSamatozinToDanaConfiguration());

            //-------------------------------------------------------------------------
            modelBuilder.ApplyConfiguration(new AccountReferenceViewConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptWithCommodityViewConfiguration());
            
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
