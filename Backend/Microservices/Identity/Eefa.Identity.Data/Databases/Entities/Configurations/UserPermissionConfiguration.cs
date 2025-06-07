using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.Identity.Data.Databases.Entities.Configurations
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.ToTable("UserPermissions", "admin");
            builder.HasComment("حدول واسط کاربران و دسترسی ها");
            builder.Property(a => a.PermissionId).IsRequired();
            builder.HasOne(a => a.Permission).WithMany(a => a.UserPermissions).HasForeignKey(a => a.PermissionId).HasPrincipalKey(a => a.Id).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(a => a.User).WithMany(a => a.UserPermissions).HasForeignKey(a => a.UserId).HasPrincipalKey(a => a.Id).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(a => a.CreatedBy).WithMany(a => a.UserPermissionCreatedBies).HasForeignKey(a => a.CreatedById).HasPrincipalKey(a => a.Id).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.ModifiedBy).WithMany(a => a.UserPermissionModifiedBies).HasForeignKey(a => a.ModifiedById).HasPrincipalKey(a => a.Id).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.OwnerRole).WithMany(a => a.UserPermissions).HasForeignKey(a => a.OwnerRoleId).HasPrincipalKey(a => a.Id).OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
