#nullable disable

using System;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Configurations
{
    public partial class AccountReferenceConfiguration : IEntityTypeConfiguration<AccountReference>
    {
        public void Configure(EntityTypeBuilder<AccountReference> entity)
        {
            entity.ToTable("AccountReferences", "accounting");

            entity.HasComment("طرف حسابها ");

            entity.HasIndex(e => e.Code)
                .IsUnique();

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("شناسه");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("فعال است؟");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("عنوان");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.AccountReferenceCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_References_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.AccountReferenceModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_References_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.AccountReferences)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_References_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<AccountReference> entity);
    }
}
