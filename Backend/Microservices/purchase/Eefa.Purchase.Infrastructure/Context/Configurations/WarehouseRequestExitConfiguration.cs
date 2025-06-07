using Eefa.Purchase.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Purchase.Infrastructure.Context.Configurations
{
    

    public partial class WarehouseRequestExitConfiguration : IEntityTypeConfiguration<WarehouseRequestExit>
    {
        public void Configure(EntityTypeBuilder<WarehouseRequestExit> entity)
        {
            entity.ToTable("WarehouseRequestExit", "inventory");

            entity.HasComment("اطلاعات درخواست خروج کالا");

            entity.Property(e => e.Id).HasComment("شناسه");



            entity.Property(e => e.Commodityld).HasComment("کد کالا");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

           

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<WarehouseRequestExit> entity);
    }
}
