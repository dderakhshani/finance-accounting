using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Admin.Data.Databases.Entities.Configurations
{
    public partial class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> entity)
        {
            entity.ToTable("MenuItems", "admin");

            entity.HasComment("منوها ");

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.FormUrl)
                .HasMaxLength(100)
                .HasComment("لینک فرم");

            entity.Property(e => e.QueryParameterMappings).HasMaxLength(3000);
            entity.Property(e => e.HelpUrl).HasMaxLength(100);

            entity.Property(e => e.ImageUrl)
                .HasMaxLength(50)
                .HasComment("لینک تصویر");

            entity.Property(e => e.IsActive).HasComment("فعال است؟");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.PageCaption)
                .HasMaxLength(100)
                .HasComment("عنوان صفحه");

            entity.Property(e => e.ParentId).HasComment("کد والد");

            entity.Property(e => e.PermissionId).HasComment("کد دسترسی");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("عنوان منو");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.MenuItemCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuItems_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.MenuItemModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_MenuItems_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuItems_Roles");

            entity.HasOne(d => d.Permission)
                .WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_MenuItems_Permissions");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<MenuItem> entity);
    }
}
