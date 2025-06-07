using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class CommodityCategoryConfiguration : IEntityTypeConfiguration<CommodityCategory>
    {
        public void Configure(EntityTypeBuilder<CommodityCategory> entity)
        {
            entity.ToTable("CommodityCategories", "common");

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.IsReadOnly).HasComment("آیا فقط قابل خواندن است؟");

            entity.Property(e => e.LevelCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("کد سطح");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OrderIndex).HasComment("ترتیب نمایش");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.ParentId).HasComment("کد والد");

            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasComment("عنوان");

            entity.Property(e => e.Code);
            entity.Property(e => e.CodingMode);
            entity.Property(e => e.RequireParentProduct);

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.CommodityCategoryCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommodityCategory_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.CommodityCategoryModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_CommodityCategory_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.CommodityCategories)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommodityCategory_Roles");

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_CommodityCategory_CommodityCategory");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CommodityCategory> entity);
    }
}
