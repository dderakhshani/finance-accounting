using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace FileTransfer.WebApi.Persistance.Entities.Configurations
{
    public partial class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> entity)
        {
            entity.ToTable("UserRoles", "admin");

            entity.HasComment("نقشهای کاربران ");

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.AllowedStatus).HasComment("وضعیت دسترسی");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.RoleId).HasComment("کد نقش");

            entity.Property(e => e.UserId).HasComment("کد کاربر");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.UserRoleCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.UserRoleModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_UserRoles_Users2");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.UserRoleOwnerRoles)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Roles1");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.UserRoleRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Roles");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserRoleUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Users");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UserRole> entity);
    }
}
