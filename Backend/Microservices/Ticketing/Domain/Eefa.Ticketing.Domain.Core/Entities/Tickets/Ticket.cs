using Eefa.Ticketing.Domain.Core.Enums;
using System.Collections.Generic;

namespace Eefa.Ticketing.Domain.Core.Entities.Tickets
{
    public class Ticket : BaseEntity
    {
        public Ticket(string title, int topicId, string pageUrl, int roleId, Priority priority, int? receiverUserId, string roleTitle, TicketDetail ticketDetail)
        {
            Title = title;
            TopicId = topicId;
            PageUrl = pageUrl;
            RoleId = roleId;/// به دلیل عدم اتصال چارت سازمانی به کاربران، نقش آنها ذخیره میشود
            Priority = priority;
            ReceiverUserId = receiverUserId;
            //CreatorUserId = creatorUserId;
            TicketDetails = new();
            TicketDetails.Add(ticketDetail);
            Status = TicketStatus.Created;
            //CreateDate = DateTime.Now;
            //UpdateDate = DateTime.Now;
            RoleTitle = roleTitle;
        }
        private Ticket()
        {

        }
        public string Title { get; private set; }
        public int TopicId { get; private set; }
        public string PageUrl { get; private set; }
        public int RoleId { get; private set; }
        public string RoleTitle { get; private set; }
        public TicketStatus Status { get; private set; }
        //public DateTime CreateDate { get; private set; }
        //public DateTime UpdateDate { get; private set; }
        public Priority Priority { get; private set; }
        public int? ReceiverUserId { get; private set; }
        //public int CreatorUserId { get; private set; }
        public List<TicketDetail> TicketDetails { get; private set; }
        public void ChangeStatus(TicketStatus status)
        {
            if (status == TicketStatus.Created)
                return;
            if ((Status == TicketStatus.Created || Status == TicketStatus.RequesterAnswered) && status == TicketStatus.Ongoing)
            {
                Status = status;
                //UpdateDate = DateTime.Now;
            }
            else if (status != TicketStatus.Ongoing)
            {
                Status = status;
                //UpdateDate = DateTime.Now;
            }
        }
    }
}
