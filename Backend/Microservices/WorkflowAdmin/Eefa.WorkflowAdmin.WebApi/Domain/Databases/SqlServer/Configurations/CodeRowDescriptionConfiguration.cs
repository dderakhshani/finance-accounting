#nullable disable

using System;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Configurations
{
    public partial class CodeRowDescriptionConfiguration : IEntityTypeConfiguration<CodeRowDescription>
    {
        public void Configure(EntityTypeBuilder<CodeRowDescription> entity)
        {
            entity.ToTable("CodeRowDescription", "accounting");

            entity.HasComment("سرح استاندارد آرتیکلهای حسابداری");

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CompanyId).HasComment("کد شرکت");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("اریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

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

            entity.HasOne(d => d.Company)
                .WithMany(p => p.CodeRowDescriptions)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodeRowDescription_CompanyInformations");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.CodeRowDescriptionCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodeRowDescription_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.CodeRowDescriptionModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_CodeRowDescription_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.CodeRowDescriptions)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodeRowDescription_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CodeRowDescription> entity);
    }
}
