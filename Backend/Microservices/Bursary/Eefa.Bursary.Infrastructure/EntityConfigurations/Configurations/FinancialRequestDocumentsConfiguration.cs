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
    public partial class FinancialRequestDocumentsConfiguration : IEntityTypeConfiguration<FinancialRequestDocuments>
    {
        public void Configure(EntityTypeBuilder<FinancialRequestDocuments> entity)
        {
            entity.ToTable("FinancialRequestDocuments", "bursary");

            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasComment("مبلغ تسویه شده از این سند ");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.DocumentHeadId).HasComment("شماره سند ");

            entity.Property(e => e.FinancialRequestId).HasComment("شماره فرم عملیات مالی ");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.SettledBaseId).HasComment("وضعیت تسویه سند ");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.FinancialRequestDocumentsCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinancialRequestDocuments_Users1");

            entity.HasOne(d => d.DocumentHead)
                .WithMany(p => p.FinancialRequestDocuments)
                .HasForeignKey(d => d.DocumentHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinancialRequestDocumentHeads_DocumentHeads");

            entity.HasOne(d => d.FinancialRequest)
                .WithMany(p => p.FinancialRequestDocuments)
                .HasForeignKey(d => d.FinancialRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinancialRequestDocuments_FinancialRequests");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.FinancialRequestDocumentsModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_FinancialRequestDocuments_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.FinancialRequestDocuments)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinancialRequestDocuments_Roles");

            entity.HasOne(d => d.SettledBase)
                .WithMany(p => p.FinancialRequestDocuments)
                .HasForeignKey(d => d.SettledBaseId)
                .HasConstraintName("FK_FinancialRequestDocumentHeads_BaseValues");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<FinancialRequestDocuments> entity);
    }
}
