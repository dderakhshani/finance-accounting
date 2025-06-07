using Eefa.Accounting.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Data.Entities.Configurations
{
    public class MapDanaAndTadbirConfiguration : IEntityTypeConfiguration<MapDanaAndTadbir>
    {
        public void Configure(EntityTypeBuilder<MapDanaAndTadbir> builder)
        {
            builder.ToTable("MapDanaAndTadbir", "common");

            builder.Property(e => e.DanaCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.TadbirCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.TadbirAccountHead)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
