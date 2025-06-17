using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Sale.Domain.Entities;

public partial class SaleUnitOfWork : DbContext
{
    public SaleUnitOfWork(DbContextOptions<SaleUnitOfWork> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<FixedPriceHistory> FixedPriceHistories { get; set; }

    public virtual DbSet<SalePriceList> SalePriceLists { get; set; }

    public virtual DbSet<SalePriceListDetail> SalePriceListDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
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
        });

        modelBuilder.Entity<FixedPriceHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FixedPrice");

            entity.ToTable("FixedPriceHistory", "Sale", tb => tb.HasComment("قیمت تمام شده"));

            entity.Property(e => e.CommodityId).HasComment("کد کالا");
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
            entity.Property(e => e.Price).HasComment("قیمت");
            entity.Property(e => e.StartDate).HasComment("تاریخ شروع");
        });

        modelBuilder.Entity<SalePriceList>(entity =>
        {
            entity.ToTable("SalePriceList", "Sale", tb =>
                {
                    tb.HasComment("لیست قیمت محصولات");
                    tb.HasTrigger("SalePriceListInsertTrriger");
                });

            entity.Property(e => e.Id).HasComment("شناسه");
            entity.Property(e => e.AccountReferenceGroupId).HasComment("نوع لیست قیمتی ");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");
            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");
            entity.Property(e => e.Descriptions)
                .HasMaxLength(500)
                .HasComment("توضیحات");
            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");
            entity.Property(e => e.LevelCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("کد سطح");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");
            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");
            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");
            entity.Property(e => e.ParentId).HasComment("کد والد");
            entity.Property(e => e.StartDate).HasComment("تاریخ شروع");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasComment("عنوان");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_SalePriceList_SalePriceList");
        });

        modelBuilder.Entity<SalePriceListDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SalePriceListDetail");

            entity.ToTable("SalePriceListDetails", "Sale", tb => tb.HasComment("لیست قیمت ریز محصولات"));

            entity.Property(e => e.Id).HasComment("شناسه");
            entity.Property(e => e.CommodityId).HasComment("کد کالا");
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
            entity.Property(e => e.SalePriceListId).HasComment("لیست قیمت فروش");
            entity.Property(e => e._ChildAccountReferenceGroupId).HasComment("نوع مشتری");

            entity.HasOne(d => d.SalePriceList).WithMany(p => p.SalePriceListDetails)
                .HasForeignKey(d => d.SalePriceListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalePriceListDetail_SalePriceList1");
        });
        modelBuilder.HasSequence("SeqPayment", "bursary");
        modelBuilder.HasSequence("SeqReceive", "bursary");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
