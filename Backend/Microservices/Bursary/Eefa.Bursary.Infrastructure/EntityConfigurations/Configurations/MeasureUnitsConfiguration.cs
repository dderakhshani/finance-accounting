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
    public partial class MeasureUnitsConfiguration : IEntityTypeConfiguration<MeasureUnits>
    {
        public void Configure(EntityTypeBuilder<MeasureUnits> entity)
        {
            entity.ToTable("MeasureUnits", "common");

            entity.HasComment("واحد های اندازه گیری");

            entity.Property(e => e.Id).HasComment("شناسه");

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

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("عنوان");

            entity.Property(e => e.UniqueName)
                .HasMaxLength(100)
                .HasComment("نام اختصاصی");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.MeasureUnitsCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeasureUnit_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.MeasureUnitsModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_MeasureUnit_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.MeasureUnits)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeasureUnit_Roles");

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_MeasureUnits_MeasureUnits");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<MeasureUnits> entity);
    }
}
