using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class AutoVoucherIncompleteVoucherConfiguration : IEntityTypeConfiguration<AutoVoucherIncompleteVoucher>
{
    public void Configure(EntityTypeBuilder<AutoVoucherIncompleteVoucher> entity)
    {
        entity.ToTable("AutoVoucherIncompleteVouchers", "accounting");

        entity.HasComment("سندهای مکانیزه کامل نشده ");

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasComment("کد");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان ایجاد");

        entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

        entity.Property(e => e.Description)
            .HasMaxLength(300)
            .HasComment("توضیحات");

        entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

        entity.Property(e => e.ModifiedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان اصلاح");

        entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

        entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

        entity.Property(e => e.RowId).HasComment("کد سطر");

        entity.Property(e => e.VoucherDate).HasComment("تاریخ سند");

        entity.Property(e => e.VoucherTypeId).HasComment("کد نوع سند");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.AutoVoucherIncompleteVoucherCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AutoVoucherIncompleteVouchers_Users");

        entity.HasOne(d => d.IdNavigation)
            .WithOne(p => p.AutoVoucherIncompleteVoucher)
            .HasForeignKey<AutoVoucherIncompleteVoucher>(d => d.Id)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AutoVoucherIncompleteVouchers_CodeVoucherGroups");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.AutoVoucherIncompleteVoucherModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_AutoVoucherIncompleteVouchers_Users1");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.AutoVoucherIncompleteVouchers)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AutoVoucherIncompleteVouchers_Roles");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<AutoVoucherIncompleteVoucher> entity);
}