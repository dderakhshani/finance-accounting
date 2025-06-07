using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Audit.Data.Databases.Entities.Configurations
{
    public partial class CodeVoucherGroupConfiguration : IEntityTypeConfiguration<CodeVoucherGroup>
    {
        public void Configure(EntityTypeBuilder<CodeVoucherGroup> entity)
        {
            entity.ToTable("CodeVoucherGroups", "accounting");

            entity.HasComment("گروههای  اسناد حسابداری");

            entity.HasIndex(e => e.Code)
                .IsUnique();

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.AutoVoucherEnterGroup).HasComment("گروه سند مکانیزه");

            entity.Property(e => e.BlankDateFormula)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("فرمول جایگزین خالی بودن");

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("شناسه");

            entity.Property(e => e.CompanyId).HasComment("کد شرکت");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.ExtendTypeName)
                .HasMaxLength(14)
                .HasComputedColumnSql("(case [ExtendTypeId] when (1) then N'افتتاحیه' when (2) then N'بستن حساب موقت' when (3) then N'اختتامیه'  end)", true);

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("فعال است؟");

            entity.Property(e => e.IsAuto).HasComment("اتوماتیک است؟");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.IsEditable).HasComment("آیا قابل ویرایش است؟");

            entity.Property(e => e.LastEditableDate).HasComment("تاریخ قفل شدن اطلاعات");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("عنوان");

            entity.Property(e => e.ViewId).HasComment("کد گزارش");

            entity.HasOne(d => d.Company)
                .WithMany(p => p.CodeVoucherGroups)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodeVoucherGroups_CompanyInformations");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.CodeVoucherGroupCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodeVoucherGroups_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.CodeVoucherGroupModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_CodeVoucherGroups_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.CodeVoucherGroups)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodeVoucherGroups_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CodeVoucherGroup> entity);
    }
}
