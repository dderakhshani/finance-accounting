using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class AutoVoucherLogConfiguration : IEntityTypeConfiguration<AutoVoucherLog>
{
    public void Configure(EntityTypeBuilder<AutoVoucherLog> entity)
    {
        entity.ToTable("AutoVoucherLog", "accounting");

        entity.HasComment("تغییرات سندهای مکانیزه ");

        entity.Property(e => e.Id).HasComment("کد");

        entity.Property(e => e.ActionDate).HasComment("تاریخ فعالیت");

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

        entity.Property(e => e.ResultId).HasComment("کد نهایی");

        entity.Property(e => e.ResultName)
            .HasMaxLength(8)
            .HasComputedColumnSql("(case [ResultId] when (1) then N'ثبت موفق' when (2) then N'بروز خطا'  end)", true)
            .HasComment("نام نهایی");

        entity.Property(e => e.RowDescription)
            .HasMaxLength(300)
            .HasComment("توضیحات سطر");

        entity.Property(e => e.VoucherDate).HasComment("تاریخ سند");

        entity.Property(e => e.VoucherTypeId).HasComment("کد نوع سند");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.AutoVoucherLogCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AutoVoucherLog_Users1");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.AutoVoucherLogModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_AutoVoucherLog_Users");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.AutoVoucherLogs)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AutoVoucherLog_Roles");

        entity.HasOne(d => d.VoucherType)
            .WithMany(p => p.AutoVoucherLogs)
            .HasForeignKey(d => d.VoucherTypeId)
            .HasConstraintName("FK_AutoVoucherLog_CodeVoucherGroups");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<AutoVoucherLog> entity);
}