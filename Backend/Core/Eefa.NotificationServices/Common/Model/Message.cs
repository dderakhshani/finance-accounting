using Eefa.NotificationServices.Common.Enum;
using System;

namespace Eefa.NotificationServices.Common.Model
{
    public class Message
    {
        public int Id { get; set; }
        public int ReceiverUserId { get; set; }
        public int MessageType { get; set; }
        public string MessageTitle { get; set; }
        public string MessageContent { get; set; }
        public string Payload { get; set; }
        public MessageStatus Status { get; set; }
        public bool SendForAllUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OwnerRoleId { get; set; }
        public int CreatedById { get; set; }
        public int ModifiedById { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
      
    }
}
    
