using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Admin.Data.Databases.Entities.Configurations
{
    public partial class RequiredPermissionConfiguration : IEntityTypeConfiguration<RequiredPermission>
    {
        public void Configure(EntityTypeBuilder<RequiredPermission> entity)
        {
            entity.ToTable("RequiredPermission", "admin");

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

            entity.Property(e => e.ParentPermissionId).HasComment("کد والد");

            entity.Property(e => e.PermissionId).HasComment("کد دسترسی");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.RequiredPermissionCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequiredPermission_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.RequiredPermissionModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_RequiredPermission_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.RequiredPermissions)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequiredPermission_Roles");

            entity.HasOne(d => d.ParentPermission)
                .WithMany(p => p.RequiredPermissionParentPermissions)
                .HasForeignKey(d => d.ParentPermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequiredPermission_Permissions");

            entity.HasOne(d => d.Permission)
                .WithMany(p => p.RequiredPermissionPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequiredPermission_RequiredPermission");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<RequiredPermission> entity);
    }
}
