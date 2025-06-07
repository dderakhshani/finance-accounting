using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Eefa.Identity.Data.Databases.Entities.Configurations
{
    public partial class UserCompanyConfiguration : IEntityTypeConfiguration<UserCompany>
    {
        public void Configure(EntityTypeBuilder<UserCompany> entity)
        {
            entity.ToTable("UserCompany", "common");

            entity.HasIndex(e => new { e.UserId, e.CompanyInformationsId })
                .IsUnique();

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.CompanyInformationsId).HasComment("کد شرکت ");

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

            entity.HasOne(d => d.CompanyInformations)
                .WithMany(p => p.UserCompanies)
                .HasForeignKey(d => d.CompanyInformationsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCompany_Company");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.UserCompanyCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCompany_Users2");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.UserCompanyModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_UserCompany_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.UserCompanies)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCompany_Roles");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserCompanyUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCompany_Users");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UserCompany> entity);
    }
}
