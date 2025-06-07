using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Admin.Data.Databases.Entities.Configurations
{
    public partial class UserSettingConfiguration : IEntityTypeConfiguration<UserSetting>
    {
        public void Configure(EntityTypeBuilder<UserSetting> entity)
        {
            entity.ToTable("UserSetting", "admin");

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.Keyword)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("کلمه کلیدی");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.UserId).HasComment("کد کاربر");

            entity.Property(e => e.Value)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("مقدار");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.UserSettingCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSetting_Users2");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.UserSettingModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_UserSetting_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.UserSettings)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSetting_Roles");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserSettingUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSetting_Users");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UserSetting> entity);
    }
}
