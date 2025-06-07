using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Audit.Data.Databases.Entities.Configurations
{
    public partial class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> entity)
        {
            entity.ToTable("Persons", "admin");

            entity.HasComment("اشخاص");

            entity.HasIndex(e => e.NationalNumber)
                .IsUnique();

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.AccountReferenceId).HasComment("کد طرف حساب");

            entity.Property(e => e.BirthDate).HasComment("تاریخ تولد");

            entity.Property(e => e.BirthPlaceCountryDivisionId).HasComment("محل تولد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .IsUnicode(false)
                .HasComment("ایمیل");

            entity.Property(e => e.FatherName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("نام پدر");

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("نام");

            entity.Property(e => e.GenderBaseId).HasComment("جنسیت");

            entity.Property(e => e.GovernmentalBaseId).HasComment("دولتی/ غیر دولتی");

            entity.Property(e => e.IdentityNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("شماره شناسنامه");

            entity.Property(e => e.InsuranceNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("شماره بیمه");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("نام خانوادگی");

            entity.Property(e => e.LegalBaseId).HasComment("حقیقی/ حقوقی");

            entity.Property(e => e.MobileJson)
                .HasMaxLength(1000)
               ;

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.NationalNumber)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("کد ملی");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.PhotoURL)
                .HasMaxLength(1000)
                .HasComment("لینک عکس شخص");

            entity.Property(e => e.SignatureURL)
                .HasMaxLength(1000)
                .HasComment("لینک امضا");

            entity.HasOne(d => d.AccountReference)
                .WithOne(p => p.Person)
                .HasConstraintName("FK_Persons_AccountReferences");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.PersonCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persons_Users");

            entity.HasOne(d => d.GenderBase)
                .WithMany(p => p.PersonGenderBases)
                .HasForeignKey(d => d.GenderBaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persons_BaseValues");

            entity.HasOne(d => d.GovernmentalBase)
                .WithMany(p => p.PersonGovernmentalBases)
                .HasForeignKey(d => d.GovernmentalBaseId)
                .HasConstraintName("FK_Persons_BaseValues1");

            entity.HasOne(d => d.LegalBase)
                .WithMany(p => p.PersonLegalBases)
                .HasForeignKey(d => d.LegalBaseId)
                .HasConstraintName("FK_Persons_BaseValues2");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.PersonModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Persons_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Persons)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persons_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Person> entity);
    }
}
