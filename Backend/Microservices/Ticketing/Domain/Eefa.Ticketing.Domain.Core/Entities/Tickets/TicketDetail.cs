using System;
using System.Collections.Generic;

namespace Eefa.Ticketing.Domain.Core.Entities.Tickets
{
    public class TicketDetail : BaseEntity
    {
        private TicketDetail()
        {

        }
        public TicketDetail(string description, string attachmentIds, int roleId)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));
            Description = description;
            AttachmentIds = attachmentIds;
            //CreatorUserId = creatorUserId;
            RoleId = roleId;
            //CreatDate = DateTime.Now;
            //CreatorUserRoleId = creatorUserRoleId;
        }

        public TicketDetail(int ticketId, string description, string attachmentIds, int roleId)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));
            Description = description;
            AttachmentIds = attachmentIds;
            //CreatorUserId = creatorUserId;
            RoleId = roleId;
            //CreatDate = DateTime.Now;
            TicketId = ticketId;
            //CreatorUserRoleId = creatorUserRoleId;
        }
        public int TicketId { get; private set; }
        public string Description { get; private set; }
        //public DateTime CreatDate { get; private set; }
        public string AttachmentIds { get; private set; }
        //public int CreatorUserId { get; private set; }
        public DateTime? ReadDate { get; private set; }
        public int RoleId { get; private set; }
        //public int CreatorUserRoleId { get; private set; }
        public int? ReaderUserId { get; private set; }
        public Ticket Ticket { get; private set; }
        public List<DetailHistory> DetailHistories { get; private set; }
        public List<PrivetMessage> PrivetMessages { get; private set; }

        public void AddDetailHistory(DetailHistory detailHistory)
        {
            if (DetailHistories == null)
                DetailHistories = new List<DetailHistory>();
            DetailHistories.Add(detailHistory);
        }
        public void AddRangDetailHistory(List<DetailHistory> detailHistories)
        {

            if (DetailHistories == null)
                DetailHistories = new List<DetailHistory>();
            DetailHistories.AddRange(detailHistories);
        }
        public void AddPrivetMessage(PrivetMessage privetMessage)
        {
            if (PrivetMessages == null)
                PrivetMessages = new List<PrivetMessage>();
            PrivetMessages.Add(privetMessage);
        }
        public void AddRangPrivetMessage(List<PrivetMessage> privetMessages)
        {
            if (PrivetMessages == null)
                PrivetMessages = new List<PrivetMessage>();
            PrivetMessages.AddRange(privetMessages);
        }
        public void ReadTicket(int readUserId)
        {
            if (readUserId == CreatedById)
                return;
            ReaderUserId = readUserId;
            ReadDate = DateTime.Now;
        }
        public void ChangeDepartment(int secondaryRoleId)
        {
            RoleId = secondaryRoleId;
            ReadDate = null;
        }
    }
}
