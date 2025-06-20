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
    public partial class EmployeesConfiguration : IEntityTypeConfiguration<Employees>
    {
        public void Configure(EntityTypeBuilder<Employees> entity)
        {
            entity.ToTable("Employees", "admin");

            entity.HasComment("کارمندان");

            entity.HasIndex(e => e.EmployeeCode)
                .HasName("IX_Employees")
                .IsUnique();

            entity.HasIndex(e => e.PersonId)
                .HasName("IX_Employees_2")
                .IsUnique();

            entity.Property(e => e.Id).HasComment("شناسه");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.EmployeeCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("کد پرسنلی");

            entity.Property(e => e.EmploymentDate).HasComment("تاریخ استخدام");

            entity.Property(e => e.Floating).HasComment("درحال جابه جایی ");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.LeaveDate).HasComment("تاریخ ترک کار");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.PersonId).HasComment("کد شخص");

            entity.Property(e => e.UnitPositionId).HasComment("کد موقعیت واحد");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Employees)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Roles");

            entity.HasOne(d => d.UnitPosition)
                .WithMany(p => p.Employees)
                .HasForeignKey(d => d.UnitPositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_UnitPositions");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Employees> entity);
    }
}
