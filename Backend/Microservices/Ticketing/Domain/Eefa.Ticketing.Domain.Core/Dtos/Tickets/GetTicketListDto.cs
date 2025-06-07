using System;

namespace Eefa.Ticketing.Domain.Core.Dtos.Tickets
{
    public class GetTicketListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int RoleId { get; set; }
        public string RoleTitle { get; set; }
        public string CreateUserTitle { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int ServiceId { get; set; }
        public int Priority { get; set; }
        public int? ReceiverUserId { get; set; }
        public int CreatorUserId { get; set; }
    }
}
