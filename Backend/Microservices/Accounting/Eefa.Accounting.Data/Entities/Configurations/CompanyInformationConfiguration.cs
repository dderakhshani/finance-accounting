using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class CompanyInformationConfiguration : IEntityTypeConfiguration<CompanyInformation>
    {
        public void Configure(EntityTypeBuilder<CompanyInformation> entity)
        {
            entity.ToTable("CompanyInformations", "common");

            entity.HasIndex(e => e.UniqueName)
                .IsUnique();

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.ExpireDate).HasComment("تاریخ انقضاء");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.Logo)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("لوگو");

            entity.Property(e => e.MaxNumOfUsers).HasComment("حداکثر تعداد کابران");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("عنوان");

            entity.Property(e => e.UniqueName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("نام یکتا");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.CompanyInformationCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyInformations_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.CompanyInformationModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_CompanyInformations_Users1");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CompanyInformation> entity);
    }
}
