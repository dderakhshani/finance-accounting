using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Eefa.Ticketing.Domain.Core.Entities.BaseInfo;


namespace Eefa.Ticketing.Infrastructure.Database.Configurations.BaseInfo
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> entity)
        {
            entity.ToTable("Persons", "admin");

            entity.HasComment("اشخاص");
            entity.HasIndex(e => e.NationalNumber)
               .IsUnique();

            entity.Property(e => e.EconomicCode)
                .HasMaxLength(50);

            entity.Property(e => e.Id).HasComment("کد");
            entity.Property(e => e.TaxIncluded);

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
                .HasMaxLength(100)
                .HasComment("نام پدر");

            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasComment("نام");

            entity.Property(e => e.GenderBaseId).HasComment("جنسیت");

            entity.Property(e => e.GovernmentalBaseId).HasComment("دولتی/ غیر دولتی");

            entity.Property(e => e.IdentityNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("شماره شناسنامه");

            entity.Property(e => e.InsuranceNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("شماره بیمه");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100)
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
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("کد ملی");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.PhotoURL)
                .HasMaxLength(1000)
                .HasComment("لینک عکس شخص");

            entity.Property(e => e.SignatureURL)
                .HasMaxLength(1000)
                .HasComment("لینک امضا");



        }

    }
}
