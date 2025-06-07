using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Identity.Data.Databases.Entities.Configurations
{
    public partial class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> entity)
        {
            entity.ToTable("RolePermissions", "admin");

            entity.HasComment("حقوق دسترسی نقشها");

            entity.Property(e => e.Id).HasComment("کد");

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

            entity.Property(e => e.PermissionId).HasComment("کد دسترسی");

            entity.Property(e => e.RoleId).HasComment("کد نقش");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.RolePermissionCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.RolePermissionModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_RolePermissions_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.RolePermissionOwnerRoles)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Roles1");

            entity.HasOne(d => d.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Permissions");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.RolePermissionRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<RolePermission> entity);
    }
}
