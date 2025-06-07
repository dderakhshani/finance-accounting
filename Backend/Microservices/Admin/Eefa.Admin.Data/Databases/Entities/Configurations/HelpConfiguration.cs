using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.Admin.Data.Databases.Entities.Configurations
{
    public partial class HelpConfiguration : IEntityTypeConfiguration<Help>
    {
        public void Configure(EntityTypeBuilder<Help> entity)
        {
            entity.ToTable("Helps", "common");

            entity.Property(e => e.Id);

            entity.Property(e => e.MenuId)
                .IsRequired();

            entity.Property(e => e.Contents)
                .IsRequired();

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند")
                .IsRequired();

            entity.Property(d => d.CreatedById).HasComment("ایجاد کننده")
                .IsRequired();

            entity.Property(e => e.CreatedAt).HasComment("تاریخ و زمان ایجاد")
                .HasDefaultValueSql("(getdate())")
                .IsRequired();

            entity.Property(d => d.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(d => d.ModifiedAt).HasComment("تاریخ و زمان اصلاح")
                .HasDefaultValueSql("(getdate())")
                .IsRequired();

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟")
                .IsRequired();


            entity.HasOne(d => d.MenuItem)
                .WithOne(p => p.Help)
                .HasForeignKey<Help>(x => x.MenuId)
                .HasConstraintName("FK_Helps_MenuItems");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.HelpCreatedBies)
                .HasForeignKey(x => x.CreatedById)
                .HasConstraintName("FK_Helps_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.HelpModifiedBies)
                .HasForeignKey(x => x.ModifiedById)
                .HasConstraintName("FK_Helps_Users1");
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Help> entity);
    }
}
