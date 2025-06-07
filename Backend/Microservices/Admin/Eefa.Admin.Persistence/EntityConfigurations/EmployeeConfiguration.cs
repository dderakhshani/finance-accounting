using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> entity)
    {
        entity.ToTable("Employees", "admin");

        entity.HasComment("کارمندان");

        entity.HasIndex(e => e.EmployeeCode)
            .IsUnique();

        entity.HasIndex(e => e.EmployeeCode)
            .IsUnique();

        entity.HasIndex(e => e.PersonId)
            .IsUnique();

        entity.Property(e => e.Id).HasComment("کد");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان ایجاد");

        entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

        entity.Property(e => e.EmployeeCode)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasComment("کد پرسنلی");

        entity.Property(e => e.EmploymentDate).HasComment("تاریخ استخدام");

        entity.Property(e => e.Floating).HasComment("شناور");

        entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

        entity.Property(e => e.LeaveDate).HasComment("تاریخ ترک کار");

        entity.Property(e => e.ModifiedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان اصلاح");

        entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

        entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

        entity.Property(e => e.PersonId).HasComment("کد پرسنلی");

        entity.Property(e => e.UnitPositionId).HasComment("کد موقعیت واحد");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.EmployeeCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Employees_Users1");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.EmployeeModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_Employees_Users");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.Employees)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Employees_Roles");

        entity.HasOne(d => d.Person)
            .WithOne(p => p.Employee)
            .HasForeignKey<Employee>(d => d.PersonId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Employees_Persons_PersonsId");

        entity.HasOne(d => d.UnitPosition)
            .WithMany(p => p.Employees)
            .HasForeignKey(d => d.UnitPositionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Employees_UnitPositions");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Employee> entity);
}