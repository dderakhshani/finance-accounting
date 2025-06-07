using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;



#nullable disable

namespace Eefa.Invertory.Infrastructure.Context.Configurations
{
    public partial class EmployeesConfiguration : IEntityTypeConfiguration<Employees>
    {
        public void Configure(EntityTypeBuilder<Employees> entity)
        {
            entity.ToTable("Employees", "admin");

            entity.HasComment("اسناد");

            entity.Property(e => e.Id).HasComment("شناسه");


            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");





            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Employees> entity);
    }
}
