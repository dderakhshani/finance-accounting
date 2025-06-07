using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class AccountReferencesGroupConfiguration : IEntityTypeConfiguration<AccountReferencesGroup>
{
    public void Configure(EntityTypeBuilder<AccountReferencesGroup> entity)
    {
        entity.ToTable("AccountReferencesGroups", "accounting");

        entity.HasComment("گروه طرف حسابها");

        entity.HasIndex(e => e.LevelCode)
            .IsUnique();

        entity.HasIndex(e => e.Title)
            .IsUnique();

        entity.Property(e => e.Id).HasComment("کد");

        entity.Property(e => e.CompanyId).HasComment("کد شرکت");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان ایجاد");

        entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

        entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

        entity.Property(e => e.IsEditable)
            .IsRequired()
            .HasDefaultValueSql("((1))")
            .HasComment("آیا قابل ویرایش است؟");

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
            .HasMaxLength(100)
            .HasComment("عنوان");

        entity.HasOne(d => d.Company)
            .WithMany(p => p.AccountReferencesGroups)
            .HasForeignKey(d => d.CompanyId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ReferencesGroups_CompanyInformations");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.AccountReferencesGroupCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ReferencesGroups_Users1");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.AccountReferencesGroupModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_AccountReferencesGroups_Users");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.AccountReferencesGroups)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ReferencesGroups_Roles");

        entity.HasOne(d => d.Parent)
            .WithMany(p => p.InverseParent)
            .HasForeignKey(d => d.ParentId)
            .HasConstraintName("FK_RefGroups_RefGroups");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<AccountReferencesGroup> entity);
}