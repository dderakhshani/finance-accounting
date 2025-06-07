#nullable disable

using System;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Configurations
{
    public partial class CommodityConfiguration : IEntityTypeConfiguration<Commodity>
    {
        public void Configure(EntityTypeBuilder<Commodity> entity)
        {
            entity.ToTable("Commodity", "common");

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CategoryId).HasComment("کد گروه");

            entity.Property(e => e.CompanyId).HasComment("کد شرکت");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasComment("توضیحات");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.LevelCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("کد سطح");

            entity.Property(e => e.MaximumQuantity).HasComment("حداکثر تعداد");

            entity.Property(e => e.MeasureId).HasComment("واحد اندازه گیری");

            entity.Property(e => e.MinimumQuantity).HasComment("حداقل تعداد");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OrderQuantity).HasComment("تعداد سفارش");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.ParentId).HasComment("کد والد");

            entity.Property(e => e.PricingType).HasComment("نوع محاسبه قیمت");

            entity.Property(e => e.ProductCode)
                .HasMaxLength(50)
                .HasComment("کد محصول");

            entity.Property(e => e.ReferenceId).HasComment("کد طرف حساب");

            entity.Property(e => e.Tite)
                .HasMaxLength(500)
                .HasComment("عنوان");

            entity.Property(e => e.TypeId).HasComment("کد نوع/ باید حذف شود");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.CommodityCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Commodity_Users");

            entity.HasOne(d => d.Measure)
                .WithMany(p => p.CommodityMeasures)
                .HasForeignKey(d => d.MeasureId)
                .HasConstraintName("FK_Commodity_BaseValues2");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.CommodityModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Commodity_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Commodities)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Commodity_Roles");

            entity.HasOne(d => d.PricingTypeNavigation)
                .WithMany(p => p.CommodityPricingTypeNavigations)
                .HasForeignKey(d => d.PricingType)
                .HasConstraintName("FK_Commodity_BaseValues1");

            entity.HasOne(d => d.Type)
                .WithMany(p => p.CommodityTypes)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_Commodity_BaseValues");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Commodity> entity);
    }
}
