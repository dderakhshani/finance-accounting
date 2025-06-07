namespace Eefa.Ticketing.Application.Contract.Dtos.Tickets
{
    public class ReplyTicketDto
    {
        public int TicketId { get; set; }
        public string Description { get; set; }
        public string AttachmentIds { get; set; }
        public int UserRoleId { get; set; }
    }
}
