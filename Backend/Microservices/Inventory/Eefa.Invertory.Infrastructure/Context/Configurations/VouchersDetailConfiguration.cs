using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Eefa.Inventory.Domain;
#nullable disable

namespace Eefa.Invertory.Infrastructure
{
    public partial class VouchersDetailConfiguration : IEntityTypeConfiguration<VouchersDetail>
    {
        public void Configure(EntityTypeBuilder<VouchersDetail> entity)
        {
            entity.HasKey(e => e.Id)
                ;
            entity.ToTable("VouchersDetail", "accounting");

            entity.HasComment("آرتیکل های سند حسابداری");

            entity.Property(e => e.AccountHeadId).HasComment("کد حساب سرپرست");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Credit).HasComment("اعتبار");

            entity.Property(e => e.Debit).HasComment("بدهکار");
            entity.Property(e => e.TraceNumber);

            entity.Property(e => e.DebitCreditStatus)
                .HasComputedColumnSql("(CONVERT([tinyint],case when isnull([debit],(0))>(0) then (1) else (2) end))", true)
                .HasComment("وضعیت مانده حساب");

            entity.Property(e => e.DocumentId).HasComment("شماره سند مرتبط ");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.Level1).HasComment("سطح 1");

            entity.Property(e => e.Level2).HasComment("سطح 2");

            entity.Property(e => e.Level3).HasComment("سطح 3");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.ReferenceDate).HasComment("تاریخ مرجع");


            entity.Property(e => e.ReferenceId1).HasComment("کد مرجع1");

            entity.Property(e => e.ReferenceId2).HasComment("کد مرجع2");

            entity.Property(e => e.ReferenceId3).HasComment("کد مرجع3");

            entity.Property(e => e.Weight).HasComment("مقدار مرجع");

            entity.Property(e => e.Remain)
                .HasComputedColumnSql("(isnull([Debit],(0))-isnull([Credit],(0)))", true)
                .HasComment("باقیمانده");

            entity.Property(e => e.RowIndex).HasComment("ترتیب سطر");

            entity.Property(e => e.VoucherDate).HasComment("تاریخ سند");

            entity.Property(e => e.VoucherId).HasComment("کد سند");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<VouchersDetail> entity);
    }
}
