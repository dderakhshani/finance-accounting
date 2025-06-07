using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.Ticketing.Infrastructure.Database.Configurations.Tickets
{
    public class PrivetMessageConfiguration : IEntityTypeConfiguration<PrivetMessage>
    {
        public void Configure(EntityTypeBuilder<PrivetMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(a => a.Message).IsRequired().HasMaxLength(1200).HasComment("پیام");
            builder.Property(a => a.TicketDetailId).IsRequired().HasComment("کلید خارجی از جزییات تیکت");
            builder.HasOne(a => a.TicketDetail).WithMany(a => a.PrivetMessages).HasForeignKey(a => a.TicketDetailId).HasPrincipalKey(a => a.Id);
            builder.HasOne(a => a.DetailHistory).WithMany(a => a.PrivetMessages).HasForeignKey(a => a.DetailHistoryId).HasPrincipalKey(a => a.Id).IsRequired(false);
        }
    }
}
