using Eefa.Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.Invertory.Infrastructure.Context.Configurations
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

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<RolePermission> entity);
    }
}
