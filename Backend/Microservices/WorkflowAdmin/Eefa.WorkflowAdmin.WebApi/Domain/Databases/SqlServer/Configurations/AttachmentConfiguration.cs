#nullable disable

using System;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Configurations
{
    public partial class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> entity)
        {
            entity.ToTable("Attachment", "admin");

            entity.Property(e => e.Id).HasComment("کد");

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
                .IsUnicode(false);

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.KeyWords)
                .HasMaxLength(250)
                .HasComment("کلمات کلیدی");

            entity.Property(e => e.LanguageId).HasComment("کد زبان");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");
            entity.Property(e => e.IsUsed).HasDefaultValue(false);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250)
                .HasComment("عنوان");

            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(1000)
                .HasComment("لینک");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.AttachmentCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachment_Users");

            entity.HasOne(d => d.Language)
                .WithMany(p => p.Attachments)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachment_Languages");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.AttachmentModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Attachment_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Attachments)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachment_Roles");

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
