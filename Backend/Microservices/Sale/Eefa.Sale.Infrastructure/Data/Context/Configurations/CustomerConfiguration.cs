﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Eefa.Sale.Infrastructure.Data.Context.Configurations
{
    public partial class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK_Customer");

            entity.ToTable("Customers", "Sale");

            entity.HasIndex(e => new { e.PersonId, e.AccountReferenceGroupId }, "IX_Customers").IsUnique();

            entity.Property(e => e.AccountReferenceGroupId)
                .HasDefaultValue(28)
                .HasComment("کد گروه مشتری ");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");
            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");
            entity.Property(e => e.CurrentAgentId).HasComment("کد اپراتور مرتبط با مشتری ");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(50)
                .HasComment("شماره مشتری");
            entity.Property(e => e.CustomerTypeBaseId).HasComment("نوع مشتری ");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasComment("توضیحات ");
            entity.Property(e => e.EconomicCode)
                .HasMaxLength(50)
                .HasComment("کد اقتصادی مشتری");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");
            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");
            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");
            entity.Property(e => e.PersonId).HasComment("کد شخص ");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Customer> entity);
    }
}
