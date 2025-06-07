using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.Ticketing.Infrastructure.Database.Configurations.Tickets
{
    public class DetailHistoryConfiguration : IEntityTypeConfiguration<DetailHistory>
    {
        public void Configure(EntityTypeBuilder<DetailHistory> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.TicketDetailId).IsRequired().HasComment("کلید خارجی از جزییات تیکت");
            builder.Property(a => a.PrimaryRoleId).IsRequired().HasComment("دپارتمان اولیه");
            builder.Property(a => a.SecondaryRoleId).IsRequired().HasComment("دپارتمان ارجاع داده شده");

            builder.HasOne(a => a.TicketDetail).WithMany(a => a.DetailHistories).HasForeignKey(a => a.TicketDetailId).HasPrincipalKey(a => a.Id);
        }
    }
}
