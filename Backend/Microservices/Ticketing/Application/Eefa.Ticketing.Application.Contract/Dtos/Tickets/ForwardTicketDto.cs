namespace Eefa.Ticketing.Application.Contract.Dtos.Tickets
{
    public class ForwardTicketDto
    {
        public int TicketDetailId { get; set; }
        public int SecondaryRoleId { get; set; }
        public string Message { get;  set; }
    }
}
