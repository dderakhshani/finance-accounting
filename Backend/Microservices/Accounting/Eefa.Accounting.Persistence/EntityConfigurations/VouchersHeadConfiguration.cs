using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class VouchersHeadConfiguration : IEntityTypeConfiguration<VouchersHead>
    {
        public void Configure(EntityTypeBuilder<VouchersHead> entity)
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_tbl_acc_VouchersHead")
                .IsClustered(false);

            entity.ToTable("VouchersHead", "accounting");

            entity.HasComment("سند حسابداری");

            entity.Property(e => e.AutoVoucherEnterGroup).HasComment("گروه سند مکانیزه");

            entity.Property(e => e.CodeVoucherGroupId).HasComment("کد گروه سند");

            entity.Property(e => e.CompanyId).HasComment("کد شرکت");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Difference)
                .HasComputedColumnSql("(isnull([TotalDebit],(0))-isnull([TotalCredit],(0)))", true)
                .HasComment("اختلاف");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.TotalCredit)
                .HasDefaultValueSql("((0))")
                .HasComment("جمع بستانکاری");

            entity.Property(e => e.TotalDebit)
                .HasDefaultValueSql("((0))")
                .HasComment("جمع بدهی");

            entity.Property(e => e.VoucherDate).HasComment("تاریخ سند");

            entity.Property(e => e.VoucherDescription)
                .IsRequired()
                .HasMaxLength(1000)
                .HasComment("شرح سند");

            entity.Property(e => e.VoucherDailyId).HasComment("کد سند");

            entity.Property(e => e.VoucherNo).HasComment("شماره سند");

            entity.Property(e => e.VoucherStateId)
                .HasDefaultValueSql("((1))")
                .HasComment("کد وضعیت سند");

            entity.Property(e => e.VoucherStateName)
                .HasMaxLength(4)
                .HasComputedColumnSql("(case [VoucherStateId] when (1) then N'موقت' when (2) then N'مرور' when (3) then N'دائم'  end)", true)
                .HasComment("نام وضعیت سند");

            entity.Property(e => e.YearId).HasComment("کد سال");

            entity.HasOne(d => d.CodeVoucherGroup)
                .WithMany(p => p.VouchersHeads)
                .HasForeignKey(d => d.CodeVoucherGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VouchersHead_CodeVoucherGroups");

            entity.HasOne(d => d.Company)
                .WithMany(p => p.VouchersHeads)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VouchersHead_CompanyInformations");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.VouchersHeadCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VouchersHead_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.VouchersHeadModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_VouchersHead_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.VouchersHeads)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VouchersHead_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<VouchersHead> entity);
    }
}
