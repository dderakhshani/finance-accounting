using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Admin.Data.Databases.Entities.Configurations
{
    public partial class CountryDivisionConfiguration : IEntityTypeConfiguration<CountryDivision>
    {
        public void Configure(EntityTypeBuilder<CountryDivision> entity)
        {
            entity.ToTable("CountryDivisions", "common");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("کد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById)
                .HasDefaultValueSql("((4))")
                .HasComment("ایجاد کننده");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById)
                .HasDefaultValueSql("((4))")
                .HasComment("اصلاح کننده");

            entity.Property(e => e.Ostan)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength(true)
                .HasComment(" کد استان");

            entity.Property(e => e.OstanTitle)
                .HasMaxLength(255)
                .HasComment("نام استان");

            entity.Property(e => e.OwnerRoleId)
                .HasDefaultValueSql("((1))")
                .HasComment("نقش صاحب سند");

            entity.Property(e => e.Shahrestan)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength(true)
                .HasComment("کد شهرستان");

            entity.Property(e => e.ShahrestanTitle)
                .HasMaxLength(255)
                .HasComment("نام شهرستان");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.CountryDivisionCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CountryDivisions_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.CountryDivisionModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_CountryDivisions_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.CountryDivisions)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CountryDivisions_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CountryDivision> entity);
    }
}
