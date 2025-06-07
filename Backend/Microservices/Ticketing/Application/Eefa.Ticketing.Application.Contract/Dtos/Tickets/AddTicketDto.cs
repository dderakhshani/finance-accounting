using Eefa.Ticketing.Domain.Core.Enums;

namespace Eefa.Ticketing.Application.Contract.Dtos.Tickets
{
    public class AddTicketDto
    {
        public string Title { get; set; }
        public int TopicId { get; set; }
        public string PageUrl { get; set; }
        public int RoleId { get; set; }
        public Priority Priority { get; set; }
        public int? ReceiverUserId { get; set; }
        public string Description { get; set; }
        public string AttachmentIds { get; set; }
        public int UserRoleId { get; set; }
    }
}
