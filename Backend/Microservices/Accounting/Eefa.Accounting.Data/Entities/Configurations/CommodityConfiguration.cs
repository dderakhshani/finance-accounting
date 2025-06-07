using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class CommodityConfiguration : IEntityTypeConfiguration<Commodity>
    {
        public void Configure(EntityTypeBuilder<Commodity> entity)
        {
            entity.ToTable("Commodities", "common");

            entity.HasComment("کالا ها");

            entity.Property(e => e.Id).HasComment("شناسه");

            entity.Property(e => e.CommodityCategoryId).HasComment("کد گروه کالا");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Descriptions)
                .HasMaxLength(1000)
                .HasComment("توضیحات");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.LevelCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("کد سطح");

            entity.Property(e => e.MaximumQuantity).HasComment("حداکثر تعداد");

            entity.Property(e => e.MeasureId).HasComment("کد واحد اندازه گیری");

            entity.Property(e => e.MinimumQuantity).HasComment("حداقل تعداد");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OrderQuantity).HasComment("تعداد سفارش");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.ParentId).HasComment("کد والد");

            entity.Property(e => e.PricingTypeBaseId).HasComment("نوع محاسبه قیمت");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasComment("کد محصول");

            entity.Property(e => e.TadbirCode)
             .HasMaxLength(50)
             .HasComment("کد تدبیر");

            entity.Property(e => e.CompactCode)
             .HasMaxLength(50);

            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .HasComment("عنوان");

            entity.Property(e => e.YearId).HasComment("کد سال");


            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Commodity_Commodity");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.CommodityCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Commodity_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.CommodityModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Commodity_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Commodities)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Commodity_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Commodity> entity);
    }
}
