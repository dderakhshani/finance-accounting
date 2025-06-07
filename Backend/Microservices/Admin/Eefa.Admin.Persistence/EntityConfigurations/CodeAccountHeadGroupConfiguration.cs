using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class CodeAccountHeadGroupConfiguration : IEntityTypeConfiguration<CodeAccountHeadGroup>
{
    public void Configure(EntityTypeBuilder<CodeAccountHeadGroup> entity)
    {
        entity.ToTable("CodeAccountHeadGroup", "accounting");

        entity.HasComment("گروه سرفصل حسابها ");

        entity.HasIndex(e => e.Code)
            .IsUnique();

        entity.Property(e => e.Id).HasComment("کد");

        entity.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasComment("شناسه");

        entity.Property(e => e.CompanyId).HasComment("کد شرکت");

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

        entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("عنوان");

        entity.HasOne(d => d.Company)
            .WithMany(p => p.CodeAccountHeadGroups)
            .HasForeignKey(d => d.CompanyId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CodeAccountHeadGroup_CompanyInformations");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.CodeAccountHeadGroupCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CodeAccountHeadGroup_Users");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.CodeAccountHeadGroupModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_CodeAccountHeadGroup_Users1");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.CodeAccountHeadGroups)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CodeAccountHeadGroup_Roles");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<CodeAccountHeadGroup> entity);
}