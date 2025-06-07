using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class DocumentPaymentConfiguration : IEntityTypeConfiguration<DocumentPayment>
    {
        public void Configure(EntityTypeBuilder<DocumentPayment> entity)
        {
            entity.ToTable("DocumentPayments", "common");

            entity.Property(e => e.Id).HasComment("شناسه");

            entity.Property(e => e.Balance).HasComment("موازنه");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.DocumentHeadId).HasComment("کد سرفصل سند");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.IsPaied)
                .HasMaxLength(10)
                .IsFixedLength(true)
                .HasComment("پرداخت شده است");

            entity.Property(e => e.LadingNo).HasComment("شماره بارگیری");

            entity.Property(e => e.LiquidationPrice).HasComment("نرخ شناور");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.PaiedDate).HasComment("تاریخ پرداخت");

            entity.Property(e => e.PaymentDate).HasComment("تاریخ سررسید");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.DocumentPaymentCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentPayment_Users");

            entity.HasOne(d => d.DocumentHead)
                .WithMany(p => p.DocumentPayments)
                .HasForeignKey(d => d.DocumentHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentPayment_DocumentHead");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.DocumentPaymentModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_DocumentPayment_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.DocumentPayments)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentPayment_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<DocumentPayment> entity);
    }
}
