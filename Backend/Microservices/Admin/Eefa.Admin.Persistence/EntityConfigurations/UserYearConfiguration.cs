using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class UserYearConfiguration : IEntityTypeConfiguration<UserYear>
{
    public void Configure(EntityTypeBuilder<UserYear> entity)
    {
        entity.ToTable("UserYear", "common");

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

        entity.Property(e => e.UserId).HasComment("کد کاربر");

        entity.Property(e => e.YearId).HasComment("کد سال");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.UserYearCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UserYear_Users2");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.UserYearModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_UserYear_Users1");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.UserYears)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UserYear_Roles");

        entity.HasOne(d => d.User)
            .WithMany(p => p.UserYearUsers)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UserYear_Users");

        entity.HasOne(d => d.Year)
            .WithMany(p => p.UserYears)
            .HasForeignKey(d => d.YearId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UserYear_Years");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<UserYear> entity);
}