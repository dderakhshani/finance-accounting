using System.Collections.Generic;

namespace Eefa.Ticketing.Domain.Core.Entities.Tickets
{
    public class DetailHistory : BaseEntity
    {
        public DetailHistory(int primaryRoleId, int secondaryRoleId, PrivetMessage privetMessage)
        {
            //CreatDate = DateTime.Now;
            PrimaryRoleId = primaryRoleId;
            SecondaryRoleId = secondaryRoleId;
            //ForwarderUserId = forwarderUserId;
            PrivetMessages = new() { privetMessage };

        }
        private DetailHistory()
        {

        }
        public int TicketDetailId { get; private set; }
        //public DateTime CreatDate { get; private set; }
        public int PrimaryRoleId { get; private set; }
        public int SecondaryRoleId { get; private set; }
        //public int ForwarderUserId { get; private set; }
        public TicketDetail TicketDetail { get; private set; }
        public List<PrivetMessage> PrivetMessages { get; private set; }
        public void EditTicketDetailId(int ticketDetailId)
        {
            TicketDetailId = ticketDetailId;
        }
        public void EditTicketDetail(TicketDetail ticketDetail)
        {
            TicketDetail = ticketDetail;
        }
    }
}
