using Eefa.NotificationServices.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.NotificationServices.Dto
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int ReceiverUserId { get; set; }
        public int MessageType { get; set; }
        public string MessageTitle { get; set; }
        public string MessageContent { get; set; }
        public string Payload { get; set; }
        public MessageStatus Status { get; set; }
        public bool SendForAllUser { get; set; }
        public string Listener { get; set; }
        public int OwnerRoleId { get; set; }   
    }
}
