﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Eefa.Invertory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using Eefa.Inventory.Domain;


#nullable disable

namespace Eefa.Invertory.Infrastructure.Context.Configurations
{
    public partial class YearConfiguration : IEntityTypeConfiguration<Year>
    {
        public void Configure(EntityTypeBuilder<Year> entity)
        {
            entity.ToTable("Years", "common");

            entity.HasComment("سال های مالی");

            entity.Property(e => e.Id).HasComment("شناسه");

            entity.Property(e => e.CompanyId).HasComment("کد شرکت");

            entity.Property(e => e.CreateWithoutStartVoucher).HasComment("ایجاد سال مالی بدون سند افتتاحیه");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.FirstDate).HasComment("تاریخ شروع");

            entity.Property(e => e.IsCalculable).HasComment("قابل شمارش است؟");

            entity.Property(e => e.IsCurrentYear).HasComment("آیا تاریخ در سال جاری است؟");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.IsEditable)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("آیا قابل ویرایش است؟");

            entity.Property(e => e.LastDate).HasComment("تاریخ پایان");

            entity.Property(e => e.LastEditableDate).HasComment("تاریخ قفل شدن اطلاعات");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.YearName).HasComment("نام سال");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Year> entity);
    }
}
