
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

#nullable disable

namespace Eefa.Invertory.Infrastructure
{
    public partial class VouchersHeadConfiguration : IEntityTypeConfiguration<Eefa.Inventory.Domain.VouchersHead>
    {
        public void Configure(EntityTypeBuilder<Eefa.Inventory.Domain.VouchersHead> entity)
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_tbl_acc_VouchersHead")
                .IsClustered(false);

            entity.ToTable("VouchersHead", "accounting");

            entity.HasComment("سند حسابداری");

            entity.Property(e => e.Id).HasComment("شناسه");

            entity.Property(e => e.AutoVoucherEnterGroup).HasComment("گروه سند مکانیزه");

            entity.Property(e => e.CodeVoucherGroupId).HasComment("کد گروه سند");

            entity.Property(e => e.CompanyId).HasComment("کد شرکت");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Difference)
                .HasComment("اختلاف")
                .HasComputedColumnSql("(isnull([TotalDebit],(0))-isnull([TotalCredit],(0)))");

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

            entity.Property(e => e.TraceNumber).HasComment("شماره سند مرتبط");

            entity.Property(e => e.VoucherDailyId).HasComment("کد سند");

            entity.Property(e => e.VoucherDate).HasComment("تاریخ سند");

            entity.Property(e => e.VoucherDescription)
                .IsRequired()
                .HasMaxLength(1000)
                .HasComment("شرح سند");

            entity.Property(e => e.VoucherNo).HasComment("شماره سند");

            entity.Property(e => e.VoucherStateId)
                .HasDefaultValueSql("((1))")
                .HasComment("کد وضعیت سند");

            entity.Property(e => e.VoucherStateName)
                .HasMaxLength(4)
                .HasComment("نام وضعیت سند")
                .HasComputedColumnSql("(case [VoucherStateId] when (1) then N'موقت' when (2) then N'مرور' when (3) then N'دائم'  end)");

            entity.Property(e => e.YearId).HasComment("کد سال");

            

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Eefa.Inventory.Domain.VouchersHead> entity);
    }
}
