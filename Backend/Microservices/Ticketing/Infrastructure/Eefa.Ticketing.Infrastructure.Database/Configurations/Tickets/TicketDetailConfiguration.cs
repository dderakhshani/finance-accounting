using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.Ticketing.Infrastructure.Database.Configurations.Tickets
{
    public class TicketDetailConfiguration : IEntityTypeConfiguration<TicketDetail>
    {
        public void Configure(EntityTypeBuilder<TicketDetail> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Description).HasMaxLength(1200).IsRequired().HasComment("توضیحات تیکت");
            builder.Property(a => a.AttachmentIds).IsRequired(false).HasComment("شناسه فایل پیوست شده");
            builder.Property(a => a.ReadDate).IsRequired(false).HasColumnType("datetime2(7)").HasComment("تاریخ مشاهده تیکت توسط دریافت کننده");
            builder.Property(a => a.RoleId).IsRequired().HasComment("دپارتمان دریافت کننده");
            builder.Property(a => a.ReaderUserId).IsRequired(false).HasComment("کاربر خواننده تیکت");

            builder.HasOne(a => a.Ticket).WithMany(a => a.TicketDetails).HasForeignKey(a => a.TicketId).HasPrincipalKey(a => a.Id);
        }
    }
}
