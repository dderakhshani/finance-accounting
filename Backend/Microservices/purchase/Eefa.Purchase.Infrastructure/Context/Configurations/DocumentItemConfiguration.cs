﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Eefa.Purchase.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using Eefa.Purchase.Domain.Aggregates.InvoiceAggregate;

#nullable disable

namespace Eefa.Purchase.Infrastructure.Context.Configurations
{
    public partial class DocumentItemConfiguration : IEntityTypeConfiguration<DocumentItem>
    {
        public void Configure(EntityTypeBuilder<DocumentItem> entity)
        {
            entity.ToTable("DocumentItems", "common");

            entity.HasComment("ریز اقلام اسناد");

            entity.Property(e => e.Id).HasComment("شناسه");
            entity.Property(e => e.MainMeasureId);
            entity.Property(e => e.ConversionRatio);

            entity.Property(e => e.CommoditySerial)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("سریال کالا");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.CurrencyPrice)
                .HasDefaultValueSql("((0))")
                .HasComment("نرخ ارز");

            entity.Property(e => e.Discount).HasComment("تخفیف");
            entity.Property(e => e.MeasureUnitConversionId);
            entity.Property(e => e.DocumentMeasureId);

            entity.Property(e => e.DocumentHeadId).HasComment("کد سرفصل سند");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.ProductionCost).HasComment("قیمت پایه");

            entity.Property(e => e.Quantity).HasComment("تعداد");

            entity.Property(e => e.UnitPrice).HasComment("قیمت واحد ");

            entity.Property(e => e.YearId).HasComment("کد سال");

            entity.HasOne(d => d.DocumentHead)
                .WithMany(p => p.Items)
                .HasForeignKey(d => d.DocumentHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Document_D_Document_H");

            entity.HasOne(d => d.Year)
                .WithMany(p => p.DocumentItems)
                .HasForeignKey(d => d.YearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentItems_Years");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<DocumentItem> entity);
    }
}
