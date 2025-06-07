using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Audit.Data.Databases.Entities.Configurations
{
    public partial class YearConfiguration : IEntityTypeConfiguration<Year>
    {
        public void Configure(EntityTypeBuilder<Year> entity)
        {
            entity.ToTable("Years", "common");

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CompanyId).HasComment("کد شرکت");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.FirstDate).HasComment("تاریخ شروع");

            entity.Property(e => e.IsCalculable).HasComment("قابل شمارش است؟");

            entity.Property(e => e.IsCurrentYear).HasComment("آیا تاریخ در سال جاری است؟");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.IsEditable)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("آیا قابل ویرایش است؟");

            entity.Property(e => e.LastDate).HasComment("تاریخ پایان");

            entity.Property(e => e.LastEditableDate).HasComment("تاریخ قفل شدن اطلاعات");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.YearName).HasComment("نام سال");

            entity.HasOne(d => d.Company)
                .WithMany(p => p.Years)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Years_CompanyInformations");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.YearCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Years_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.YearModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Years_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Years)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Years_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Year> entity);
    }
}
