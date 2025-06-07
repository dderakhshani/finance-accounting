using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.Ticketing.Infrastructure.Database.Configurations.Tickets
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(a => a.Title).IsRequired().HasMaxLength(100).HasComment("عنوان تیکت");
            builder.Property(a => a.Status).IsRequired().HasComment("وضعیت:" +
                " ایجاد شده=0،" +
                " پاسخ کاربر ایجاد کننده=1،" +
                " پاسخ کاربر دریافت کننده=2،" +
                " ارجاع داده شده=3،" +
                " در دست اقدام=4،" +
                " بسته شده توسط کاربر ایجاد کننده=5،" +
                " بسته شده توسط کاربر پاسخ دهنده=6" +
                "بسته شده توسط سیستم=7");
            builder.Property(a => a.RoleId).IsRequired().HasComment("دپارتمان دریافت کننده");
            builder.Property(a => a.RoleTitle).IsRequired().HasComment("نام دپارتمان دریافت کننده");
            builder.Property(a => a.ReceiverUserId).IsRequired(false).HasComment("کاربر دریافت کننده");
            builder.Property(a => a.Priority).IsRequired().HasComment("اولیت");
            builder.Property(a => a.PageUrl).IsRequired(false).HasMaxLength(500).HasComment("صفحه ای که روی آن تیکت خورده");
            builder.HasIndex(a => a.RoleId);
        }
    }
}
