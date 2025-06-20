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
    public partial class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> entity)
        {
            entity.ToTable("Attachment", "admin");

            entity.HasComment("پیوست ها");

            entity.Property(e => e.Id).HasComment("شناسه");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Description)
                .HasMaxLength(3000)
                .HasComment("توضیحات");

            entity.Property(e => e.Extention)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("نوع پسوند فایل");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.IsUsed).HasComment("استفاد شده است ");

            entity.Property(e => e.KeyWords)
                .HasMaxLength(250)
                .HasComment("کلمات کلیدی");

            entity.Property(e => e.LanguageId).HasComment("کد زبان");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250)
                .HasComment("عنوان");

            entity.Property(e => e.TypeBaseId).HasComment("کد نوع");

            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(1000)
                .HasComment("لینک");

            entity.HasOne(d => d.Language)
                .WithMany(p => p.Attachments)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachment_Languages");

            entity.HasOne(d => d.TypeBase)
                .WithMany(p => p.Attachments)
                .HasForeignKey(d => d.TypeBaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachment_BaseValues");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Attachment> entity);
    }
}
