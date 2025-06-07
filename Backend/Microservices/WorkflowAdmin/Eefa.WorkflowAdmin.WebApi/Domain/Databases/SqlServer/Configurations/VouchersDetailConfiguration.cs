#nullable disable

using System;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Configurations
{
    public partial class VouchersDetailConfiguration : IEntityTypeConfiguration<VouchersDetail>
    {
        public void Configure(EntityTypeBuilder<VouchersDetail> entity)
        {
            entity.HasKey(e => e.Id)
                .IsClustered(false);

            entity.ToTable("VouchersDetail", "accounting");

            entity.HasComment("آرتیکل های سند حسابداری");

            entity.Property(e => e.AccountHeadId).HasComment("کد حساب سرپرست");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Credit).HasComment("اعتبار");

            entity.Property(e => e.Debit).HasComment("بدهکار");

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

            entity.Property(e => e.ReferenceId).HasComment("شماره مرجع");

            entity.Property(e => e.ReferenceId1).HasComment("کد مرجع1");

            entity.Property(e => e.ReferenceId2).HasComment("کد مرجع2");

            entity.Property(e => e.ReferenceId3).HasComment("کد مرجع3");

            entity.Property(e => e.ReferenceQty).HasComment("مقدار مرجع");

            entity.Property(e => e.Remain)
                .HasComputedColumnSql("(isnull([Debit],(0))-isnull([Credit],(0)))", true)
                .HasComment("باقیمانده");

            entity.Property(e => e.RowIndex).HasComment("ترتیب سطر");

            entity.Property(e => e.VoucherDate).HasComment("تاریخ سند");

            entity.Property(e => e.VoucherId).HasComment("کد سند");

            entity.Property(e => e.VoucherRowDescription)
                .IsRequired()
                .HasMaxLength(300)
                .HasComment("شرح آرتیکل  سند");

            entity.HasOne(d => d.AccountHead)
                .WithMany(p => p.VouchersDetails)
                .HasForeignKey(d => d.AccountHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VouchersDetail_AccountHead");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.VouchersDetailCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VouchersDetail_Users");

            entity.HasOne(d => d.Document)
                .WithMany(p => p.VouchersDetails)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_VouchersDetail_DocumentHead");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.VouchersDetailModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_VouchersDetail_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.VouchersDetails)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VouchersDetail_Roles");


            entity.HasOne(d => d.ReferenceId1Navigation)
                .WithMany(p => p.VouchersDetailReferenceId1Navigation)
                .HasForeignKey(d => d.ReferenceId1)
                .HasConstraintName("FK_VouchersDetail_References");

            entity.HasOne(d => d.ReferenceId2Navigation)
                .WithMany(p => p.VouchersDetailReferenceId2Navigation)
                .HasForeignKey(d => d.ReferenceId2)
                .HasConstraintName("FK_VouchersDetail_References1");

            entity.HasOne(d => d.ReferenceId3Navigation)
                .WithMany(p => p.VouchersDetailReferenceId3Navigation)
                .HasForeignKey(d => d.ReferenceId3)
                .HasConstraintName("FK_VouchersDetail_References2");

            entity.HasOne(d => d.Voucher)
                .WithMany(p => p.VouchersDetails)
                .HasForeignKey(d => d.VoucherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VouchersDetail_VouchersHead");

            entity.HasOne(d => d.AccountReferencesGroup)
                .WithMany(p => p.VouchersDetails)
                .HasForeignKey(d => d.AccountReferencesGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VouchersDetail_AccountReferencesGroups");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<VouchersDetail> entity);
    }
}
