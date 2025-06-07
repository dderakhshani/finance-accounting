using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Data.Events.Abstraction
{
    public partial class ApplicationEventConfigurations : IEntityTypeConfiguration<ApplicationEvent>
    {
        public void Configure(EntityTypeBuilder<ApplicationEvent> entity)
        {
            entity.ToTable("ApplicationEvents", "dbo");

            entity.HasComment("رویدادها");

            entity.HasKey(x => x.Id).IsClustered(false);

            entity.Property(e => e.EntityType)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.Descriptions)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.ApplicationEventCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationEvents_Users");


            entity.HasOne(d => d.Origin)
                .WithMany(p => p.SubEvents)
                .HasForeignKey(d => d.OriginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationEvents_ApplicationEvents");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ApplicationEvent> entity);
    }
}

