using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class DocumentItemConfiguration : IEntityTypeConfiguration<DocumentItem>
    {
        public void Configure(EntityTypeBuilder<DocumentItem> entity)
        {
            entity.ToTable("DocumentItems", "common");

            entity.HasIndex(e => e.DocumentHeadId);

            entity.Property(e => e.Id).HasComment("شناسه");

            entity.Property(e => e.ProductionCost).HasComment("قیمت پایه");

            entity.Property(e => e.CommoditySerial)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("سریال کالا");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.CurrencyPrice)
                .HasDefaultValueSql("((0))")
                .HasComment("نرخ ارز");

            entity.Property(e => e.Discount).HasComment("تخفیف");

            entity.Property(e => e.DocumentHeadId).HasComment("کد سرفصل سند");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

         

            entity.Property(e => e.Quantity).HasComment("تعداد");


            entity.Property(e => e.UnitPrice).HasComment("قیمت واحد ");

            entity.Property(e => e.YearId).HasComment("کد سال");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.DocumentItemCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentItems_Users");

            entity.HasOne(d => d.DocumentHead)
                .WithMany(p => p.DocumentItems)
                .HasForeignKey(d => d.DocumentHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Document_D_Document_H");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.DocumentItemModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_DocumentItems_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.DocumentItems)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentDetail_Roles");

            entity.HasOne(d => d.Year)
                .WithMany(p => p.DocumentItems)
                .HasForeignKey(d => d.YearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentItems_Years");

            entity.HasOne(d => d.CurrencyBase)
                .WithMany(p => p.DocumentItems)
                .HasForeignKey(d => d.CurrencyBaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentItems_BaseValues");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<DocumentItem> entity);
    }
}

