using System;

namespace Eefa.Ticketing.Domain.Core.Entities.Tickets
{
    public class PrivetMessage : BaseEntity
    {
        public PrivetMessage(int ticketDetailId, string message, int? detailHistoryId)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));
            //CreatorUserId = creatorUserId;
            Message = message;
            TicketDetailId = ticketDetailId;
            //CreatDate = DateTime.Now;
            DetailHistoryId = detailHistoryId;
        }
        public PrivetMessage(string message, int? detailHistoryId)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));
            //CreatorUserId = creatorUserId;
            Message = message;
            //CreatDate = DateTime.Now;
            DetailHistoryId = detailHistoryId;
        }
        private PrivetMessage()
        {

        }
        public int TicketDetailId { get; private set; }
        //public int CreatorUserId { get; private set; }
        public string Message { get; private set; }
        //public DateTime CreatDate { get; private set; }
        public int? DetailHistoryId { get; private set; }
        public TicketDetail TicketDetail { get; private set; }
        public DetailHistory DetailHistory { get; private set; }
    }
}
