namespace Eefa.Ticketing.Domain.Core.Enums
{
    public enum TicketStatus
    {
        Created = 0,
        RequesterAnswered = 1,
        ReceiverAnswered = 2,
        Forwarded = 3,
        Ongoing = 4,
        ClosedByRequester = 5,
        ClosedByReceiver = 6,
        ClosedBySystem = 7
    }
}
