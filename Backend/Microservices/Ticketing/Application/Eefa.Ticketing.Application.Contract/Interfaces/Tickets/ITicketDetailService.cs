using Eefa.Ticketing.Application.Contract.Dtos.Tickets;
using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Contract.Interfaces.Tickets
{
    public interface ITicketDetailService //: IBaseService<TicketDetail>
    {
        Task<int> ReadTicket(int id, int userId, CancellationToken cancellationToken);
        Task<int> ReplyTicket(ReplyTicketDto replyTicketDto, int userId, CancellationToken cancellationToken);
        Task<int> ForwardTicketAsync(ForwardTicketDto forwardTicketDto, int userId, CancellationToken cancellationToken);
        Task<List<GetTicketDetailListDto>> GetList(int ticketId, int userId, CancellationToken cancellationToken);
    }
}
