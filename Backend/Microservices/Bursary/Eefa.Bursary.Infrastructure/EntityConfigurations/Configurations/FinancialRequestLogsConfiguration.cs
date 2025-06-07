using Eefa.Bursary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Infrastructure.EntityConfigurations.Configurations
{
    public partial class FinancialRequestLogsConfiguration : IEntityTypeConfiguration<FinancialRequestLogs>
    {
        public void Configure(EntityTypeBuilder<FinancialRequestLogs> entity)
        {

            entity.ToTable("FinancialRequestLogs", "bursary");

            entity.Property(e => e.RequestType)
               .IsRequired()
               .HasMaxLength(250);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.FinancialLogsCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinancialRequestLogs_Users");

            OnConfigurePartial(entity);

        }

        partial void OnConfigurePartial(EntityTypeBuilder<FinancialRequestLogs> entity);

    }
}
 
