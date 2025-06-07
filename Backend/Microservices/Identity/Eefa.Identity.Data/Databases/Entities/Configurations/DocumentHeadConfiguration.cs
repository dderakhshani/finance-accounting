using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Identity.Data.Databases.Entities.Configurations
{
    public partial class DocumentHeadConfiguration : IEntityTypeConfiguration<DocumentHead>
    {
        public void Configure(EntityTypeBuilder<DocumentHead> entity)
        {
            entity.ToTable("DocumentHead", "common");

            entity.HasIndex(e => new { e.CompanyId, e.FormNo });

            entity.HasIndex(e => e.StoreId);

            entity.HasIndex(e => e.ParentId);

            entity.HasIndex(e => e.PaymentDate);

            entity.HasIndex(e => e.PaymentTypeId);

            entity.HasIndex(e => e.Id);

            entity.HasIndex(e => e.FormDate);

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.Balance).HasComment("موازنه");

            entity.Property(e => e.CompanyId).HasComment("کد شرکت");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.FormDate).HasComment("تاریخ فرم");

            entity.Property(e => e.FormDescription)
                .HasMaxLength(300)
                .HasComment("توضیحات فرم");

            entity.Property(e => e.FormNo).HasComment("شماره فرم");

            entity.Property(e => e.FormStateId)
                .HasDefaultValueSql("((1))")
                .HasComment("کد وضعیت فرم");

            entity.Property(e => e.FormTypeId)
                .HasDefaultValueSql("((300))")
                .HasComment("کد نوع فرم");

            entity.Property(e => e.InvoiceTypeId).HasComment("کد نوع حساب");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.IsManual).HasComment("آیا دستی است؟");

            entity.Property(e => e.IsPaied)
                .HasComputedColumnSql("(case when (((isnull([TotalItemPrice],(0))-isnull([TotalDiscount],(0)))+isnull([Totaltax],(0)))-isnull([LiquidationPrice],(0)))=(0) then (1) else (0) end)", false)
                .HasComment("آیا پرداخت شده است؟");

            entity.Property(e => e.LadingNo).HasComment("شماره بارگذاری");

            entity.Property(e => e.LiquidationPrice).HasComment("نرخ شناور");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.PaiedDate).HasComment("تاریخ پرداخت");

            entity.Property(e => e.ParentId).HasComment("کد والد");

            entity.Property(e => e.PayDescription)
                .HasMaxLength(500)
                .HasComment("شرح پرداخت");

            entity.Property(e => e.PaymentDate).HasComment("تاریخ سررسید");

            entity.Property(e => e.PaymentTypeId)
                .HasDefaultValueSql("((1))")
                .HasComment("نوع پرداخت");

            entity.Property(e => e.PriceMinusDiscount)
                .HasComputedColumnSql("(isnull([TotalItemPrice],(0))-isnull([TotalDiscount],(0)))", false)
                .HasComment("قیمت بعد از کسر تخفیف");

            entity.Property(e => e.PriceMinusDiscountPlusTax)
                .HasComputedColumnSql("((isnull([TotalItemPrice],(0))-isnull([TotalDiscount],(0)))+isnull([Totaltax],(0)))", false)
                .HasComment("قیمت با مالیات بعد از کسر تخفیف ");

            entity.Property(e => e.ReferenceId).HasComment("کد مرجع");

            entity.Property(e => e.StoreId).HasComment("کد فروشگاه");

            entity.Property(e => e.TotalDiscount).HasComment("جمع تخفیف");

            entity.Property(e => e.TotalItemPrice).HasComment("جمع کل سند");

            entity.Property(e => e.TotalTax).HasComment("جمع مالیات");

            entity.Property(e => e.TotalVat).HasComment("جمع ارزش افزوده");

            entity.Property(e => e.TypeId).HasComment("نوع");

            entity.Property(e => e.VocherId).HasComment("کد سند");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.DocumentHeadCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentHead_Users");

            entity.HasOne(d => d.FormType)
                .WithMany(p => p.DocumentHeadFormTypes)
                .HasForeignKey(d => d.FormTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentHead_BaseValues");

            entity.HasOne(d => d.InvoiceType)
                .WithMany(p => p.DocumentHeadInvoiceTypes)
                .HasForeignKey(d => d.InvoiceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentHead_BaseValues2");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.DocumentHeadModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_DocumentHead_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.DocumentHeads)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentHead_Roles");

            entity.HasOne(d => d.PaymentType)
                .WithMany(p => p.DocumentHeadPaymentTypes)
                .HasForeignKey(d => d.PaymentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentHead_BaseValues3");

            entity.HasOne(d => d.Type)
                .WithMany(p => p.DocumentHeadTypes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentHead_BaseValues1");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<DocumentHead> entity);
    }
}
