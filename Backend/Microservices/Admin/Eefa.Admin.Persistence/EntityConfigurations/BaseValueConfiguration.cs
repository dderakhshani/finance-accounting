using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class BaseValueConfiguration : IEntityTypeConfiguration<BaseValue>
{
    public void Configure(EntityTypeBuilder<BaseValue> entity)
    {
        entity.ToTable("BaseValues", "admin");

        entity.HasComment("اطلاعات پایه ");

        entity.HasIndex(e => e.LevelCode)
            .IsUnique();

        entity.HasIndex(e => new { e.Code, e.BaseValueTypeId })
            .IsUnique();

        entity.HasIndex(e => e.UniqueName)
            .IsUnique();

        entity.Property(e => e.Id).HasComment("کد");

        entity.Property(e => e.BaseValueTypeId).HasComment("کد نوع مقدار");

        entity.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasComment("شناسه");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان ایجاد");

        entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

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

        entity.Property(e => e.OrderIndex).HasComment("ترتیب آرتیکل سند حسابداری");

        entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

        entity.Property(e => e.ParentId).HasComment("کد والد");

        entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(250)
            .HasComment("عنوان");

        entity.Property(e => e.UniqueName)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("نام یکتا");

        entity.Property(e => e.Value)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("مقدار");

        entity.HasOne(d => d.BaseValueType)
            .WithMany(p => p.BaseValues)
            .HasForeignKey(d => d.BaseValueTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_BaseValues_BaseValueTypes");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.BaseValueCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_BaseValues_Users");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.BaseValueModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_BaseValues_Users1");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<BaseValue> entity);
}