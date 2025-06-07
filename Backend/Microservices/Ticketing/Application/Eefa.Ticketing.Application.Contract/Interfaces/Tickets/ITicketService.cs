using Eefa.Ticketing.Application.Contract.Dtos.Tickets;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Domain.Core.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Contract.Interfaces.Tickets
{
    public interface ITicketService //: IBaseService<Ticket>
    {
        Task<int> Add(AddTicketDto addTicketDto, int creatorUserId, CancellationToken cancellationToken);
        Task CloseTicket(int ticketId, int creatorUserId, CancellationToken cancellationToken);
        Task ChangeTicketStatusAsync(Ticket ticket, int ticketId, TicketStatus ticketStatus, CancellationToken cancellationToken);
        //Task<List<GetTicketListDto>> GetList(CancellationToken cancellationToken);
    }
}
