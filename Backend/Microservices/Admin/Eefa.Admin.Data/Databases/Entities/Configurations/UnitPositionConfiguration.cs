using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Admin.Data.Databases.Entities.Configurations
{
    public partial class UnitPositionConfiguration : IEntityTypeConfiguration<UnitPosition>
    {
        public void Configure(EntityTypeBuilder<UnitPosition> entity)
        {
            entity.ToTable("UnitPositions", "admin");

            entity.HasComment("مشاغل یک واحد ");

            entity.HasIndex(e => e.Id)
                .IsUnique();

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

            entity.Property(e => e.PositionId).HasComment("کد موقعیت شغلی");

            entity.Property(e => e.UnitId).HasComment("کد واحد");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.UnitPositionCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitPositions_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.UnitPositionModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_UnitPositions_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.UnitPositions)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitPositions_Roles");

            entity.HasOne(d => d.Position)
                .WithMany(p => p.UnitPositions)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK_UnitPositions_Positions");

            entity.HasOne(d => d.Unit)
                .WithMany(p => p.UnitPositions)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitPositions_Units");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UnitPosition> entity);
    }
}
