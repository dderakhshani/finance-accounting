using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace FileTransfer.WebApi.Persistance.Entities.Configurations
{
    public partial class BaseValueTypeConfiguration : IEntityTypeConfiguration<BaseValueType>
    {
        public void Configure(EntityTypeBuilder<BaseValueType> entity)
        {
            entity.ToTable("BaseValueTypes", "admin");

            entity.HasComment("نوع اطلاعات پایه ");

            entity.HasIndex(e => e.LevelCode)
                .IsUnique();

            entity.HasIndex(e => e.UniqueName)
                .IsUnique();

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .HasComment("نام گروه");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.IsReadOnly).HasComment("آیا فقط قابل خواندن است؟");

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

            entity.Property(e => e.SubSystem)
                .HasMaxLength(50)
                .HasComment("زیر سیستم");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250)
                .HasComment("عنوان");

            entity.Property(e => e.UniqueName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("نام یکتا");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.BaseValueTypeCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BaseValueTypes_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.BaseValueTypeModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_BaseValueTypes_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.BaseValueTypes)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BaseValueTypes_Roles");

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_BaseValueTypes_BaseValueTypes1");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<BaseValueType> entity);
    }
}
