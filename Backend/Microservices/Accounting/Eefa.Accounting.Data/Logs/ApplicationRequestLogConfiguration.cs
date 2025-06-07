using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Data.Logs
{
    public partial class ApplicationRequestLogConfiguration : IEntityTypeConfiguration<ApplicationRequestLog>
    {
        public void Configure(EntityTypeBuilder<ApplicationRequestLog> entity)
        {
            entity.Property(e => e.RequestType)
               .IsRequired()
               .HasMaxLength(250);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.ApplicationRequestLogCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationRequestLogs_Users");

            OnConfigurePartial(entity);

        }

        partial void OnConfigurePartial(EntityTypeBuilder<ApplicationRequestLog> entity);

    }
}
