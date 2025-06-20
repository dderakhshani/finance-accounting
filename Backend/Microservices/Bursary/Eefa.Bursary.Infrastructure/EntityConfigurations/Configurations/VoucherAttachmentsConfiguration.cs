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
    public partial class VoucherAttachmentsConfiguration : IEntityTypeConfiguration<VoucherAttachments>
    {
        public void Configure(EntityTypeBuilder<VoucherAttachments> entity)
        {
            entity.ToTable("VoucherAttachments", "accounting");

            entity.HasIndex(e => new { e.VoucherHeadId, e.AttachmentId })
                .HasName("IX_VoucherAttachments")
                .IsUnique();

            entity.Property(e => e.Id).HasComment("شناسه");

            entity.Property(e => e.AttachmentId).HasComment("کد پیوست");

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

            entity.Property(e => e.VoucherHeadId).HasComment("کد فایل راهنما");

            entity.HasOne(d => d.Attachment)
                .WithMany(p => p.VoucherAttachments)
                .HasForeignKey(d => d.AttachmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VoucherAttachments_Attachment");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.VoucherAttachments)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VoucherAttachments_Roles");

            entity.HasOne(d => d.VoucherHead)
                .WithMany(p => p.VoucherAttachments)
                .HasForeignKey(d => d.VoucherHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Voucher_VoucherAttachments");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<VoucherAttachments> entity);
    }
}
