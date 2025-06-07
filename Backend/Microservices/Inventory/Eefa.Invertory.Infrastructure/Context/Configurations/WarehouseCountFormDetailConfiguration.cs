using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;

namespace Eefa.Invertory.Infrastructure.Context.Configurations
{
    public partial class WarehouseCountFormDetailConfiguration : IEntityTypeConfiguration<WarehouseCountFormDetail>
    {
        public void Configure(EntityTypeBuilder<WarehouseCountFormDetail> entity)
        {
            entity.ToTable("WarehouseCountFormDetail", "inventory");
            entity.Property(e => e.Id).HasComment("شناسه");
            entity.Property(e => e.WarehouseCountFormHeadId).HasComment("شماره فرم والد ");
            entity.Property(e => e.WarehouseLayoutQuantitiesId).HasComment("کد موجودی لوکیشن انبار ");
            entity.Property(e => e.CountedQuantity).HasComment("مقدار شمارش شده");
            entity.Property(e => e.Description).HasComment("توضیحات ");           
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");
            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");
            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");
            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");
            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");            
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<WarehouseCountFormDetail> entity);
    }
}
