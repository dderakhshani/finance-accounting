﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Eefa.Bursary.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using Eefa.Bursary.Domain.Entities;
#nullable disable

namespace Eefa.Bursary.Infrastructure.EntityConfigurations.Configurations
{
    public partial class AccountHeadRelReferenceGroupConfiguration : IEntityTypeConfiguration<AccountHeadRelReferenceGroup>
    {
        public void Configure(EntityTypeBuilder<AccountHeadRelReferenceGroup> entity)
        {
            entity.ToTable("AccountHeadRelReferenceGroup", "accounting");

            entity.HasComment("نگاشت بین سرفصل حسابها و گروه طرف حسابها ");

            entity.Property(e => e.Id).HasComment("شناسه");

            entity.Property(e => e.AccountHeadId).HasComment("سرفصل حساب ");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.IsCredit).HasComment("معتبر است؟");

            entity.Property(e => e.IsDebit).HasComment("بدهکار است؟");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.ReferenceGroupId).HasComment("کد گروه مرجع");

            entity.Property(e => e.ReferenceNo).HasComment("شماره مرجع");

            entity.HasOne(d => d.AccountHead)
                .WithMany(p => p.AccountHeadRelReferenceGroups)
                .HasForeignKey(d => d.AccountHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountHeadRelReferenceGroup_AccountHead");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.AccountHeadRelReferenceGroupCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountHeadRelReferenceGroup_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.AccountHeadRelReferenceGroupModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_AccountHeadRelReferenceGroup_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.AccountHeadRelReferenceGroups)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountHeadRelReferenceGroup_Roles");

            entity.HasOne(d => d.ReferenceGroup)
                .WithMany(p => p.AccountHeadRelReferenceGroups)
                .HasForeignKey(d => d.ReferenceGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountHeadRelReferenceGroup_ReferencesGroups");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<AccountHeadRelReferenceGroup> entity);
    }
}
