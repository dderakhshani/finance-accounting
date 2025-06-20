﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class AutoVoucherFormulaConfiguration : IEntityTypeConfiguration<AutoVoucherFormula>
{
    public void Configure(EntityTypeBuilder<AutoVoucherFormula> entity)
    {
        entity.HasKey(e => e.Id)
            .IsClustered(false);

        entity.ToTable("AutoVoucherFormula", "accounting");

        entity.HasComment("فرمول سند اتو ماتیک");

        entity.Property(e => e.Id).HasComment("کد");

        entity.Property(e => e.AccountHeadId).HasComment("کد سطح");

        entity.Property(e => e.ConditionPart)
            .HasMaxLength(300)
            .HasComment("شرط");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان ایجاد");

        entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

        entity.Property(e => e.DebitCreditStatus).HasComment("وضعیت مانده حساب");

        entity.Property(e => e.DestinationFields)
            .HasMaxLength(300)
            .HasComment("ستونهای مقصد");

        entity.Property(e => e.GroupBy)
            .HasMaxLength(200)
            .IsUnicode(false)
            .HasComment("گروه شده با");

        entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

        entity.Property(e => e.ModifiedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان اصلاح");

        entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

        entity.Property(e => e.OrderBy)
            .HasMaxLength(200)
            .IsUnicode(false)
            .HasComment("ترتیب");

        entity.Property(e => e.OrderIndex).HasComment("ترتیب آرتیکل سند حسابداری");

        entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

        entity.Property(e => e.RowDescription)
            .HasMaxLength(200)
            .HasComment("توضیحات سطر");

        entity.Property(e => e.SourceFields)
            .HasMaxLength(500)
            .HasComment("ستونهای مبداء");

        entity.Property(e => e.VoucherTypeId).HasComment("کد نوع سند");

        entity.HasOne(d => d.AccountHead)
            .WithMany(p => p.AutoVoucherFormulas)
            .HasForeignKey(d => d.AccountHeadId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AutoVoucherFormula_AccountHead");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.AutoVoucherFormulaCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AutoVoucherFormula_Users1");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.AutoVoucherFormulaModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_AutoVoucherFormula_Users");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.AutoVoucherFormulas)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AutoVoucherFormula_Roles");

        entity.HasOne(d => d.VoucherType)
            .WithMany(p => p.AutoVoucherFormulas)
            .HasForeignKey(d => d.VoucherTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AutoVoucherFormula_CodeVoucherGroups");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<AutoVoucherFormula> entity);
}