using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Audit.Data.Databases.Entities.Configurations
{
    public partial class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> entity)
        {
            entity.ToTable("Branches", "admin");

            entity.HasComment("شعبه ها ");

            entity.HasIndex(e => e.LevelCode)
                .IsUnique();

            entity.HasIndex(e => e.Title)
                .IsUnique();

            entity.Property(e => e.Id).HasComment("کد");

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

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.BranchCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branches_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.BranchModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Branches_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Branches)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branches_Roles");

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Branches_Branches");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Branch> entity);
    }
}
