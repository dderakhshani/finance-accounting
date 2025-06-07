using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;

namespace Eefa.Invertory.Infrastructure.Context.Configurations
{
    public partial class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users", "admin");

            entity.HasComment("اطلاعات کاربران در این جدول ذخیره میشود ");

            entity.HasIndex(e => e.PasswordExpiryDate);
            entity.HasIndex(e => e.Username)
                .IsUnique();

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
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<User> entity);
    }
}
