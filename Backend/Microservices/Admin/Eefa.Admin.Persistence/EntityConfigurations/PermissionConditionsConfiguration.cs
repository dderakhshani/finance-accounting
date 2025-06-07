using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PermissionConditionsConfiguration : IEntityTypeConfiguration<PermissionCondition>
{
    public void Configure(EntityTypeBuilder<PermissionCondition> builder)
    {
        builder.ToTable("PermissionConditions", "admin");
        builder.HasComment("شرط های دسترسی ها");
        builder.Property(a => a.Condition).HasMaxLength(1000);
        builder.Property(a => a.JsonCondition).IsRequired(false).HasMaxLength(2000);
        //builder.HasOne(a => a.Table).WithMany(a => a.PermissionConditions).HasForeignKey(a => a.TableId).IsRequired(false).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a => a.MenuItem).WithMany(a => a.PermissionConditions).HasForeignKey(a => a.MenuId).IsRequired(false).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a => a.Permission).WithMany(a => a.PermissionConditions).HasForeignKey(a => a.PermissionId).IsRequired(false).OnDelete(DeleteBehavior.Cascade);
    }
}