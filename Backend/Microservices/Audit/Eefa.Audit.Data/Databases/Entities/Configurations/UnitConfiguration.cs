using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Audit.Data.Databases.Entities.Configurations
{
    public partial class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> entity)
        {
            entity.ToTable("Units", "admin");

            entity.HasComment("واحد ها ");

            entity.HasIndex(e => e.Title)
                .IsUnique();

            entity.HasIndex(e => e.LevelCode)
                .IsUnique();

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.BranchId).HasComment("کد شعبه");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.LevelCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasComment("کد سطح");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.ParentId).HasComment("کد والد");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("عنوان");

            entity.HasOne(d => d.Branch)
                .WithMany(p => p.Units)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Units_Branches");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.UnitCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Units_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.UnitModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Units_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Units)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Units_Roles");

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Unit> entity);
    }
}
