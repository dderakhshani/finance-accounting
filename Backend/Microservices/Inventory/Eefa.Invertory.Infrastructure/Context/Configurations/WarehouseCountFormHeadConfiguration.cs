using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;

namespace Eefa.Invertory.Infrastructure.Context.Configurations
{
    public partial class WarehouseCountFormHeadConfiguration : IEntityTypeConfiguration<WarehouseCountFormHead>
    {
        public void Configure(EntityTypeBuilder<WarehouseCountFormHead> entity)
        {
            entity.ToTable("WarehouseCountFormHead", "inventory");
            entity.Property(e => e.Id).HasComment("شناسه");
            entity.Property(e => e.ParentId).HasComment("شماره فرم والد ");
            entity.Property(e => e.FormDate).HasComment("شماره پایان");
            entity.Property(e => e.CounterUserId).HasComment("کد کاربر شمارش کننده ");
            entity.Property(e => e.ConfirmerUserId).HasComment("کد کاربر شمارش کننده ");
            entity.Property(e => e.FormState);
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
            entity.Property(e => e.ParentId).HasComment("کد والد");
            entity.Property(e => e.WarehouseId).HasComment("کد انبار");
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<WarehouseCountFormHead> entity);
    }
}
