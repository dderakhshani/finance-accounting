﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> entity)
    {
        entity.ToTable("Permissions", "admin");

        entity.HasComment("حقوق دسترسی");

        entity.HasIndex(e => e.UniqueName)
            .IsUnique();

        entity.HasIndex(e => e.LevelCode)
            .IsUnique();

        entity.Property(e => e.Id).HasComment("کد");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان ایجاد");

        entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

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

        entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

        entity.Property(e => e.ParentId).HasComment("کد والد");

        entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("عنوان");

        entity.Property(e => e.UniqueName)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("نام یکتا");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.PermissionCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Permissions_Users1");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.PermissionModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_Permissions_Users");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.Permissions)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Permissions_Roles");

        entity.HasOne(d => d.Parent)
            .WithMany(p => p.InverseParent)
            .HasForeignKey(d => d.ParentId);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Permission> entity);
}