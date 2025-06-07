using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Domain.Core.Interfaces.Tickets
{
    public interface ITicketDetailRepository
    {
        Task<List<GetTicketDetailListDto>> GetList(int ticketId, CancellationToken cancelationtoken);
        Task<List<TicketDetail>> GetUnreadListAsync(int ticketId, CancellationToken cancelationtoken);
        Task<GetCreatorUserAndRoleIdDto> GetCreatorUserAndRoleId(int id, CancellationToken cancelationtoken);
    }
}
