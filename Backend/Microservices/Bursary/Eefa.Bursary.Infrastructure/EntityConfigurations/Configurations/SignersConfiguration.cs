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
    public partial class SignersConfiguration : IEntityTypeConfiguration<Signers>
    {
        public void Configure(EntityTypeBuilder<Signers> entity)
        {
            entity.ToTable("Signers", "admin");

            entity.HasComment("امضا کنندگان");

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.ActiveDate).HasComment("تاریخ فعال شدن");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.ExpireDate).HasComment("تاریخ غیر فعال شدن");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("فعال");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.PersonId).HasComment("کد شخص");

            entity.Property(e => e.SignerDescription)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("عنوان امضاء کننده ");

            entity.Property(e => e.SignerOrderIndex).HasComment("چندمین امضاء کننده");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Signers)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Signers_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Signers> entity);
    }
}
