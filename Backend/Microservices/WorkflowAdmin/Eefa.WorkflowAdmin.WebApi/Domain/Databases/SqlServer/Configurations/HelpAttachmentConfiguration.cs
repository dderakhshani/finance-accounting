#nullable disable

using System;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Configurations
{
    public partial class HelpAttachmentConfiguration : IEntityTypeConfiguration<HelpAttachment>
    {
        public void Configure(EntityTypeBuilder<HelpAttachment> entity)
        {
            entity.ToTable("HelpAttachment", "admin");

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.AttachmentId).HasComment("کد پیوست");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.HelpDataId).HasComment("کد فایل راهنما");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.HasOne(d => d.Attachment)
                .WithMany(p => p.HelpAttachments)
                .HasForeignKey(d => d.AttachmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HelpAttachment_Attachment");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.HelpAttachmentCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HelpAttachment_Users");

            entity.HasOne(d => d.HelpData)
                .WithMany(p => p.HelpAttachments)
                .HasForeignKey(d => d.HelpDataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HelpAttachment_HelpData");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.HelpAttachmentModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_HelpAttachment_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.HelpAttachments)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HelpAttachment_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<HelpAttachment> entity);
    }
}
