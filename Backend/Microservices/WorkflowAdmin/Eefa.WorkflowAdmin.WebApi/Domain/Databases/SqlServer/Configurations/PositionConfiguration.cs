#nullable disable

using System;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Configurations
{
    public partial class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> entity)
        {
            entity.ToTable("Positions", "admin");

            entity.HasComment("مشاغل");

            entity.HasIndex(e => e.Title)
                .IsUnique();

            entity.HasIndex(e => e.LevelCode)
                .IsUnique();

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById)
                .HasDefaultValueSql("((1))")
                .HasComment("ایجاد کننده");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.LevelCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasComment("کد سطح");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId)
                .HasDefaultValueSql("((1))")
                .HasComment("نقش صاحب سند");

            entity.Property(e => e.ParentId).HasComment("کد والد");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("عنوان");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.PositionCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Positions_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.PositionModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Positions_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Positions)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Positions_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Position> entity);
    }
}
