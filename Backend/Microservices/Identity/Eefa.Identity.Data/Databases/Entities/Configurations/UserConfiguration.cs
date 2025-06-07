using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Identity.Data.Databases.Entities.Configurations
{
    public partial class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users", "admin");

            entity.HasComment("اطلاعات کاربران در این جدول ذخیره میشود ");

            entity.HasIndex(e => e.Username)
                .IsUnique();
            entity.HasIndex(e => e.PasswordExpiryDate);

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.BlockedReasonBaseId).HasComment("علت قفل شدن");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.FailedCount).HasComment("دفعات ورود ناموفق");

            entity.Property(e => e.IsBlocked).HasComment("آیا قفل شده است؟");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.LastOnlineTime).HasComment("آخرین زمان آنلاین بودن");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OneTimePassword)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("رمز یکبار مصرف");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("رمز");

            entity.Property(e => e.PersonId).HasComment("کد پرسنلی");

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("نام کاربری");

            entity.HasOne(d => d.BlockedReasonBase)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.BlockedReasonBaseId)
                .HasConstraintName("FK_Users_BaseValues");

            entity.HasOne(d => d.Person)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<User> entity);
    }
}
