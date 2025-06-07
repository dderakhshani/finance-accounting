#nullable disable

using System;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Configurations
{
    public partial class PersonAddressConfiguration : IEntityTypeConfiguration<PersonAddress>
    {
        public void Configure(EntityTypeBuilder<PersonAddress> entity)
        {
            entity.ToTable("PersonAddress", "admin");

            entity.HasComment("آدرسهای اشخاص ");

            entity.Property(e => e.Id).HasComment("کد");

            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasComment("آدرس");

            entity.Property(e => e.IsDefault)
                .HasDefaultValueSql("((0))")
                .HasComment("آدرس پیش فرض ");

            entity.Property(e => e.CountryDivisionId).HasComment("کد شهرستان");

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

            entity.Property(e => e.PersonId).HasComment("کد والد");

            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength(true)
                .HasComment("کد پستی");

            entity.Property(e => e.TelephoneJson)
                .HasMaxLength(1000)
                .HasComment("تلفن");

            entity.Property(e => e.TypeBaseId).HasComment("عنوان آدرس");

            entity.HasOne(d => d.CountryDivision)
                .WithMany(p => p.PersonAddresses)
                .HasForeignKey(d => d.CountryDivisionId)
                .HasConstraintName("FK_PersonAddress_CountryDivisions");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.PersonAddressCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonAddress_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.PersonAddressModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_PersonAddress_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.PersonAddresses)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonAddress_Roles");

            entity.HasOne(d => d.Person)
                .WithMany(p => p.PersonAddresses)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonAddress_Persons");

            entity.HasOne(d => d.TypeBase)
                .WithMany(p => p.PersonAddresses)
                .HasForeignKey(d => d.TypeBaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonAddress_BaseValues");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PersonAddress> entity);
    }
}
