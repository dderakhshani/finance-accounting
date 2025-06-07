using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable


namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class PersonPhonesConfiguration : IEntityTypeConfiguration<PersonPhone>
    {
        public void Configure(EntityTypeBuilder<PersonPhone> entity)
        {
            entity.ToTable("PersonPhones", "admin");

            entity.Property(e => e.Id)
                .HasComment("شناسه");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasComment("توضیحات");

            entity.Property(e => e.IsDefault).HasComment("تلفن پیش فرض ");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.PersonId).HasComment("کد شخص");

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(25)
                .HasComment("شماره تلفن ");

            entity.Property(e => e.PhoneTypeBaseId).HasComment("عنوان تلفن ");



            entity.HasOne(d => d.Person)
                .WithMany(p => p.PersonPhones)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonPhones_Persons");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PersonPhone> entity);
    }
}
