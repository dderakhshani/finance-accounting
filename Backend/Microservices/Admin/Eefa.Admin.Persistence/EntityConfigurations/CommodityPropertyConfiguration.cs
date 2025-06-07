using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class CommodityPropertyConfiguration : IEntityTypeConfiguration<CommodityProperty>
{
    public void Configure(EntityTypeBuilder<CommodityProperty> entity)
    {
        entity.ToTable("CommodityProperties", "common");

        entity.Property(e => e.CategoryPropertyId).HasComment("کد گروه کالا");

        entity.Property(e => e.CommodityId).HasComment("کد کالا");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان ایجاد");

        entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

        entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

        entity.Property(e => e.ModifiedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان اصلاح");

        entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

        entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

        entity.Property(e => e.Value).HasComment("مقدار");

        entity.Property(e => e.ValueBaseId).HasComment("واحد اندازه گیری مقدار");

        entity.HasOne(d => d.CategoryProperty)
            .WithMany(p => p.CommodityProperties)
            .HasForeignKey(d => d.CategoryPropertyId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CommodityProperties_CommodityCategoryProperty");

        entity.HasOne(d => d.Commodity)
            .WithMany(p => p.CommodityProperties)
            .HasForeignKey(d => d.CommodityId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CommodityProperties_Commodity");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.CommodityPropertyCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CommodityProperties_Users");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.CommodityPropertyModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_CommodityProperties_Users1");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.CommodityProperties)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CommodityProperties_Roles");

        entity.HasOne(d => d.ValueBase)
            .WithMany(p => p.CommodityProperties)
            .HasForeignKey(d => d.ValueBaseId)
            .HasConstraintName("FK_CommodityProperties_BaseValues");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<CommodityProperty> entity);
}