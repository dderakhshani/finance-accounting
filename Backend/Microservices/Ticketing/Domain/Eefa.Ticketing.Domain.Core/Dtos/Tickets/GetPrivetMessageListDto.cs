using System;

namespace Eefa.Ticketing.Domain.Core.Dtos.Tickets
{
    public class GetPrivetMessageListDto
    {
        public int Id { get; set; }
        public int TicketDetailId { get; set; }
        public int CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public string Message { get; set; }
        public DateTime CreatDate { get; set; }
    }
}
