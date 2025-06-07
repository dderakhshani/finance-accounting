#nullable disable

using System;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Configurations
{
    public partial class DocumentItemConfiguration : IEntityTypeConfiguration<DocumentItem>
    {
        public void Configure(EntityTypeBuilder<DocumentItem> entity)
        {
            entity.ToTable("DocumentItem", "common");

            entity.HasIndex(e => e.DocumentHeadId);

            entity.HasIndex(e => new { e.CommodityId, e.FirstDocumentHeadId, e.ParentId });

            entity.Property(e => e.BasePrice).HasComment("قیمت پایه");

            entity.Property(e => e.CommodityBachNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("شماره بچ کالا");

            entity.Property(e => e.CommodityId).HasComment("کد کالا");

            entity.Property(e => e.CommoditySerial)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("سریال");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.CurrencyPrice)
                .HasDefaultValueSql("((0))")
                .HasComment("نرخ ارز");

            entity.Property(e => e.Discount).HasComment("تخفیف");

            entity.Property(e => e.DocumentHeadId).HasComment("کد سرفصل سند");

            entity.Property(e => e.EquipmentId).HasComment("کد تجهیزات");

            entity.Property(e => e.EquipmentSerial)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("سریال تجهیزات");

            entity.Property(e => e.ExpireDate).HasComment("تاریخ انقضا");

            entity.Property(e => e.FirstDocumentHeadId).HasComment("اولین کد سرفصل سند");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.LadingBillNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("شماره بارنامه");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.ParentId).HasComment("کد والد");

            entity.Property(e => e.PartNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("شماره بخش");

            entity.Property(e => e.Quantity).HasComment("تعداد");

            entity.Property(e => e.Tax).HasComment("مالیات");

            entity.Property(e => e.UnitPrice).HasComment("قیمت هر واحد");

            entity.HasOne(d => d.Commodity)
                .WithMany(p => p.DocumentItems)
                .HasForeignKey(d => d.CommodityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentItem_Items");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.DocumentItemCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentItem_Users");

            entity.HasOne(d => d.DocumentHead)
                .WithMany(p => p.DocumentItems)
                .HasForeignKey(d => d.DocumentHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Document_D_Document_H");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.DocumentItemModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_DocumentItem_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.DocumentItems)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentItem_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<DocumentItem> entity);
    }
}
