using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Identity.Data.Databases.Entities.Configurations
{
    public partial class ShiftInfoConfiguration : IEntityTypeConfiguration<ShiftInfo>
    {
        public void Configure(EntityTypeBuilder<ShiftInfo> entity)
        {
            entity.ToTable("ShiftInfo", "admin");

            entity.HasIndex(e => e.Title)
                .IsUnique();

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.EndTime).HasComment("پایان شیفت");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.StartTime).HasComment("شروع شیفت");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("عنوان");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.ShiftInfoCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShiftInfo_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.ShiftInfoModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_ShiftInfo_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.ShiftInfoes)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShiftInfo_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ShiftInfo> entity);
    }
}
