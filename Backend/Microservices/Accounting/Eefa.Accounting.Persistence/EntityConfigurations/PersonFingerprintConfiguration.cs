using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class PersonFingerprintConfiguration : IEntityTypeConfiguration<PersonFingerprint>
    {
        public void Configure(EntityTypeBuilder<PersonFingerprint> entity)
        {
            entity.ToTable("PersonFingerprint", "admin");

            entity.HasComment("اثر انگشت");

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.FingerBaseId).HasComment("شماره انگشت");

            entity.Property(e => e.FingerPrintPhotoURL)
                .HasMaxLength(1000)
                .HasComment("عکس اثر انگشت");

            entity.Property(e => e.FingerPrintTemplate)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("الگوی اثر انگشت");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.PersonId).HasComment("کد پرسنلی");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.PersonFingerprintCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonFingerprint_Users1");

            entity.HasOne(d => d.FingerBase)
                .WithMany(p => p.PersonFingerprints)
                .HasForeignKey(d => d.FingerBaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonFingerprint_BaseValues");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.PersonFingerprintModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_PersonFingerprint_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.PersonFingerprints)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonFingerprint_Roles");

            entity.HasOne(d => d.Person)
                .WithMany(p => p.PersonFingerprints)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonFingerprint_Persons");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PersonFingerprint> entity);
    }
}
