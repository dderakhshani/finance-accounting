using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class CodeVoucherExtendTypeConfiguration : IEntityTypeConfiguration<CodeVoucherExtendType>
    {
        public void Configure(EntityTypeBuilder<CodeVoucherExtendType> entity)
        {
            entity.ToTable("CodeVoucherExtendType", "accounting");

            entity.Property(e => e.Id).HasComment("کد");

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

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("عنوان");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.CodeVoucherExtendTypeCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodeVoucherExceptionType_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.CodeVoucherExtendTypeModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_CodeVoucherExceptionType_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.CodeVoucherExtendTypes)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodeVoucherExceptionType_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CodeVoucherExtendType> entity);
    }
}
