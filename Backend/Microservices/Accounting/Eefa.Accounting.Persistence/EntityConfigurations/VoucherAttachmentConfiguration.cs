using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class VoucherAttachmentConfiguration : IEntityTypeConfiguration<VoucherAttachment>
    {
        public void Configure(EntityTypeBuilder<VoucherAttachment> entity)
        {
            entity.ToTable("VoucherAttachments", "accounting");

            entity.HasIndex(e => new { e.VoucherHeadId, e.AttachmentId })
                .IsUnique();

            entity.Property(e => e.Id)
                .HasComment("کد");

            entity.Property(e => e.AttachmentId).HasComment("کد پیوست");

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

            entity.Property(e => e.VoucherHeadId).HasComment("کد فایل راهنما");

            entity.HasOne(d => d.Attachment)
                .WithMany(p => p.VoucherAttachments)
                .HasForeignKey(d => d.AttachmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VoucherAttachments_Attachment");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.VoucherAttachmentCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VoucherAttachments_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.VoucherAttachmentModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_VoucherAttachments_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.VoucherAttachments)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VoucherAttachments_Roles");

            entity.HasOne(d => d.VoucherHead)
                .WithMany(p => p.VoucherAttachments)
                .HasForeignKey(d => d.VoucherHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Voucher_VoucherAttachments");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<VoucherAttachment> entity);
    }
}
