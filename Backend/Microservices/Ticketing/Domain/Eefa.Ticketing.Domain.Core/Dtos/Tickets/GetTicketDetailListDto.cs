using Eefa.Ticketing.Domain.Core.Enums;
using System;

namespace Eefa.Ticketing.Domain.Core.Dtos.Tickets
{
    public class GetTicketDetailListDto
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string TicketTitle { get; set; }
        public string Description { get; set; }
        public DateTime CreatDate { get; set; }
        public string AttachmentIds { get; set; }
        public int DetailCreatorUserId { get; set; }
        public string DetailCreatorUserFullName { get; set; }
        public int TicketCreatorUserId { get; set; }
        public string TicketCreatorUserFullName { get; set; }
        public DateTime? ReadDate { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int DetailCreatorUserRoleId { get; set; }
        public string DetailCreatorUserRoleName { get; set; }
        public int? ReaderUserId { get; set; }
        public string ReaderUserFullName { get; set; }
        public int HistoryCount { get; set; }
        public TicketStatus TicketStatus { get; set; }
    }
}
