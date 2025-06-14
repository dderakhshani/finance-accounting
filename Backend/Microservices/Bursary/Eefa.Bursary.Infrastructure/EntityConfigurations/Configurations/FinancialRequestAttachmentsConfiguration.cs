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
    public partial class FinancialRequestAttachmentsConfiguration : IEntityTypeConfiguration<FinancialRequestAttachments>
    {
        public void Configure(EntityTypeBuilder<FinancialRequestAttachments> entity)
        {
            entity.ToTable("FinancialRequestAttachments", "bursary");

            entity.HasComment("فایل های درخواست مالی");

            entity.Property(e => e.AttachmentId).HasComment("شماره ضمیمه ");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.FinancialRequestId).HasComment("شماره درخواست");

            entity.Property(e => e.IsVerified).HasComment("آیا ضمیمه تایید شده هست ؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.HasOne(d => d.Attachment)
                .WithMany(p => p.FinancialRequestAttachments)
                .HasForeignKey(d => d.AttachmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinancialRequestAttachments_Attachment");


            entity.HasOne(d => d.ChequeSheet)
                .WithMany(p => p.FinancialRequestAttachments)
                .HasForeignKey(d => d.ChequeSheetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinancialRequestAttachments_ChequeSheet");


            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.FinancialRequestAttachmentsCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinancialRequestAttachments_Users1");

            entity.HasOne(d => d.FinancialRequest)
                .WithMany(p => p.FinancialRequestAttachments)
                .HasForeignKey(d => d.FinancialRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinancialRequestAttachments_FinancialRequests");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.FinancialRequestAttachmentsModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_FinancialRequestAttachments_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.FinancialRequestAttachments)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinancialRequestAttachments_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<FinancialRequestAttachments> entity);
    }
}
