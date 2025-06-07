using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Audit.Data.Databases.Entities.Configurations
{
    public partial class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> entity)
        {
            entity.ToTable("Languages", "admin");

            entity.HasComment("زبانها");

            entity.Property(e => e.Id)
                .HasComment("کد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Culture)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("نماد");

            entity.Property(e => e.DefaultCurrencyBaseId).HasComment("واحد پول پیش فرض");

            entity.Property(e => e.DirectionBaseId).HasComment("راست چین");

            entity.Property(e => e.FlagImageUrl)
                .HasMaxLength(500)
                .HasComment("نماد پرچم کشور");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.SeoCode)
                .HasMaxLength(2)
                .HasComment("کد سئو");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("عنوان");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.LanguageCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Languages_Users");

            entity.HasOne(d => d.DirectionBase)
                .WithMany(p => p.DirectionLanguages)
                .HasForeignKey(d => d.DirectionBaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Languages_BaseValues");

            entity.HasOne(d => d.DefaultCurrencyBase)
                .WithMany(p => p.DefaultCurrencyLanguages)
                .HasForeignKey(d => d.DefaultCurrencyBaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Languages_BaseValues1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.LanguageModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Languages_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Languages)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Languages_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Language> entity);
    }
}
