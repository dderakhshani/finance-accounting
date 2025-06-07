using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Audit.Data.Databases.Entities.Configurations
{
    public partial class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> entity)
        {
            entity.ToTable("Holidays", "accounting");

            entity.HasIndex(e => e.Date)
                .IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Date).HasPrecision(0);

            entity.Property(e => e.Description).HasMaxLength(250);

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.HolidayCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Holidays_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.HolidayModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Holidays_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Holidays)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Holidays_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Holiday> entity);
    }
}
