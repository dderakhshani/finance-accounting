using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class DocumentHeadConfiguration : IEntityTypeConfiguration<DocumentHead>
    {
        public void Configure(EntityTypeBuilder<DocumentHead> entity)
        {
            entity.ToTable("DocumentHeads", "common");

            entity.HasIndex(e => new { e.YearId, e.DocumentNo });

            entity.HasIndex(e => e.WarehouseId);
            entity.Property(e => e.DocumentSerial);

            entity.HasIndex(e => e.ParentId);

            entity.HasIndex(e => e.PaymentTypeBaseId);

            entity.HasIndex(e => e.Id);

            entity.HasIndex(e => e.DocumentDate);

            entity.Property(e => e.Id).HasComment("شناسه");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.DocumentDescription)
                .HasMaxLength(500)
                .HasComment("توضیحات");

            entity.Property(e => e.DiscountPercent).HasComment("درصد تخفیف");

            entity.Property(e => e.DocumentDate).HasComment("تاریخ سند");


            entity.Property(e => e.DocumentDiscount).HasComment("تخفیف کل سند");

            entity.Property(e => e.DocumentNo).HasComment("شماره سند");

            entity.Property(e => e.DocumentStateBaseId)
                .HasDefaultValueSql("((1))")
                .HasComment("کد وضعیت سند");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.IsManual).HasComment("دستی");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.ParentId).HasComment("کد والد");

            entity.Property(e => e.PaymentTypeBaseId)
                .HasDefaultValueSql("((1))")
                .HasComment("نوع پرداخت");

            entity.Property(e => e.PriceMinusDiscount)
                .HasComputedColumnSql("(isnull([TotalItemPrice],(0))-isnull([TotalItemsDiscount],(0)))", false)
                .HasComment("قیمت بعد از کسر تخفیف");

            entity.Property(e => e.PriceMinusDiscountPlusTax)
                .HasComputedColumnSql("((isnull([TotalItemPrice],(0))-isnull([TotalItemsDiscount],(0)))+isnull([VatTax],(0)))", false)
                .HasComment("قیمت با مالیات بعد از کسر تخفیف ");

            entity.Property(e => e.ReferenceId).HasComment("کد مرجع");

            entity.Property(e => e.TotalItemPrice).HasComment("جمع مبلغ سند");

            entity.Property(e => e.TotalItemsDiscount).HasComment("جمع تخفیف");

            entity.Property(e => e.VatTax).HasComment("جمع مالیات");


            entity.Property(e => e.WarehouseId).HasComment("کد انبار");

            entity.Property(e => e.YearId).HasComment("کد سال");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.DocumentHeadCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentHead_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.DocumentHeadModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_DocumentHead_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.DocumentHeads)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentHead_Roles");

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_DocumentHead_DocumentHead");

            //entity.HasOne(d => d.PaymentTypeBase)
            //    .WithMany(p => p.DocumentHeads)
            //    .HasForeignKey(d => d.PaymentTypeBaseId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_DocumentHead_BaseValues3");

            entity.HasOne(d => d.Reference)
                .WithMany(p => p.DocumentHeads)
                .HasForeignKey(d => d.ReferenceId)
                .HasConstraintName("FK_DocumentHead_AccountReferences");

            //entity.HasOne(d => d.AccountReferencesGroup)
            //    .WithMany(p => p.DocumentHeads)
            //    .HasForeignKey(d => d.AccountReferencesGroupId)
            //    .HasConstraintName("FK_DocumentHead_AccountReferencesGroup");   
            
            
            entity.HasOne(d => d.DocumentStateBase)
                .WithMany(p => p.DocumentHeads)
                .HasForeignKey(d => d.DocumentStateBaseId)
                .HasConstraintName("FK_DocumentHeads_BaseValues");

            entity.HasOne(d => d.Year)
                .WithMany(p => p.DocumentHeads)
                .HasForeignKey(d => d.YearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentHead_Years");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<DocumentHead> entity);
    }
}

