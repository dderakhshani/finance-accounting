using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public partial class HelpDataConfiguration : IEntityTypeConfiguration<HelpData>
    {
        public void Configure(EntityTypeBuilder<HelpData> entity)
        {
            entity.ToTable("HelpData", "admin");

            entity.HasIndex(e => e.LevelCode)
                .IsUnique();

            entity.HasIndex(e => e.Title)
                .IsUnique();

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Description)
                .HasMaxLength(3000)
                .HasComment("توضیحات");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.KeyWords)
                .HasMaxLength(250)
                .HasComment("کلمه کلیدی");

            entity.Property(e => e.LanguageId).HasComment("کد زبان");

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
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasComment("عنوان");

            entity.Property(e => e.Url)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasComment("لینک");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.HelpDataCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HelpData_Users");

            entity.HasOne(d => d.Language)
                .WithMany(p => p.HelpDatas)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("FK_HelpData_Languages");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.HelpDataModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_HelpData_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.HelpDatas)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HelpData_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<HelpData> entity);
    }
}
