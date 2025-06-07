using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Identity.Data.Databases.Entities.Configurations
{
    public partial class AutoVoucherRowsLinkConfiguration : IEntityTypeConfiguration<AutoVoucherRowsLink>
    {
        public void Configure(EntityTypeBuilder<AutoVoucherRowsLink> entity)
        {
            entity.ToTable("AutoVoucherRowsLink", "accounting");

            entity.HasComment("ارتباط آرتیکل سندهای مکانیزه ");

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

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.AutoVoucherRowsLinkCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AutoVoucherRowsLink_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.AutoVoucherRowsLinkModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_AutoVoucherRowsLink_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.AutoVoucherRowsLinks)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AutoVoucherRowsLink_Roles");

            entity.HasOne(d => d.VoucherType)
                .WithMany(p => p.AutoVoucherRowsLinks)
                .HasForeignKey(d => d.VoucherTypeId)
                .HasConstraintName("FK_AutoVoucherRowsLink_CodeVoucherGroups");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<AutoVoucherRowsLink> entity);
    }
}
