using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class AccountReferencesRelReferencesGroupConfiguration : IEntityTypeConfiguration<AccountReferencesRelReferencesGroup>
{
    public void Configure(EntityTypeBuilder<AccountReferencesRelReferencesGroup> entity)
    {
        entity.ToTable("AccountReferencesRelReferencesGroups", "accounting");

        entity.HasComment("ارتباط بین طرف حسابها و گروه طرف حساب");

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

        entity.Property(e => e.ReferenceGroupId).HasComment("کد گروه طرف حساب");

        entity.Property(e => e.ReferenceId).HasComment("کد طرف حساب");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.AccountReferencesRelReferencesGroupCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ReferencesRelReferencesGroups_Users1");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.AccountReferencesRelReferencesGroupModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_ReferencesRelReferencesGroups_Users");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.AccountReferencesRelReferencesGroups)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ReferencesRelReferencesGroups_Roles");

        entity.HasOne(d => d.ReferenceGroup)
            .WithMany(p => p.AccountReferencesRelReferencesGroups)
            .HasForeignKey(d => d.ReferenceGroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ReferencesRelReferencesGroups_ReferencesGroups");

        entity.HasOne(d => d.Reference)
            .WithMany(p => p.AccountReferencesRelReferencesGroups)
            .HasForeignKey(d => d.ReferenceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ReferencesRelReferencesGroups_References");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<AccountReferencesRelReferencesGroup> entity);
}