using System;

namespace Eefa.Ticketing.Domain.Core.Dtos.Tickets
{
    public class GetTicketDetailHistoryListDto
    {
        public int HistoryId { get; set; }
        public int TicketDetailId { get; set; }
        public DateTime CreatDate { get; set; }
        public int PrimaryRoleId { get; set; }
        public int SecondaryRoleId { get; set; }
        public string PrimaryRoleName { get; set; }
        public string SecondaryRoleName { get; set; }
        public string Message { get; set; }
    }
}
