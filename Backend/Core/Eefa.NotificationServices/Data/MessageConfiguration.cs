using Eefa.NotificationServices.Common.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.NotificationServices.Data
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {           
            builder.ToTable("Message", "Common");
            builder.HasKey(e => e.Id).HasName("pk_ah_1");
            builder.Property(e => e.Id).HasComment("شناسه");
            builder.Property(e => e.ReceiverUserId)
            .HasComment("دریافت کننده پیام");

        
            builder.Property(e=>e.MessageType)
                .HasComment("نوع ارسال پیام  \r\n1 Signal R\r\n2 SMS\r\n3 Email");
            builder.Property(e => e.MessageContent).HasComment("محتوای پیام ");
            builder.Property(e=>e.MessageTitle).HasComment("عنوان پیام ");
            builder.Property(e => e.Payload).HasComment("Changed Oject Json ");
            builder.Property(e => e.Status).HasComment("وضعیت پیام \r\n1 ارسال شده \r\n2 خوانده شده \r\n");
            builder.Property(e => e.SendForAllUser).HasComment("ارسال برای همه ");
            builder.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند ");
            builder.Property(e => e.CreatedById).HasComment("ایجاد کننده ");
            builder.Property(e => e.CreatedAt).HasComment("تاریخ و زمان ایجاد ");
            builder.Property(e => e.ModifiedById).HasComment(" اصلاح کننده");
            builder.Property(e => e.ModifiedAt).HasComment("تاریخ و زمان اصلاح");
            builder.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");
        }
    }
}
