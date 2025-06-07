using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Domain.Core.Interfaces.Tickets
{
    public interface ITicketRepository
    {
        Task<int> GetRoleId(int ticketId);
        Task<List<GetTicketListDto>> GetList(GetTicketListInputDto input, List<int> roleIds, CancellationToken cancellationToken);
    }
}
