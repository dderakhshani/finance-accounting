using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

public partial class SignerConfiguration : IEntityTypeConfiguration<Signer>
{
    public void Configure(EntityTypeBuilder<Signer> entity)
    {
        entity.ToTable("Signers", "admin");

        entity.HasComment("امضا کنندگان");

        entity.Property(e => e.Id).HasComment("کد");

        entity.Property(e => e.ActiveDate).HasComment("تاریخ فعال شدن");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان ایجاد");

        entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

        entity.Property(e => e.ExpireDate).HasComment("تاریخ غیر فعال شدن");

        entity.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValueSql("((1))")
            .HasComment("فعال است؟");

        entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

        entity.Property(e => e.ModifiedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان اصلاح");

        entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

        entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

        entity.Property(e => e.PersonId).HasComment("کد پرسنلی");

        entity.Property(e => e.SignerDescription)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("عنوان امضاء کننده ");

        entity.Property(e => e.SignerOrderIndex).HasComment("چندمین امضاء کننده");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.SignerCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Signers_Users1");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.SignerModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_Signers_Users");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.Signers)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Signers_Roles");

        entity.HasOne(d => d.Person)
            .WithMany(p => p.Signers)
            .HasForeignKey(d => d.PersonId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Signers_Persons");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Signer> entity);
}