using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using System.Collections.Generic;
using System.Threading;

namespace Eefa.Ticketing.Domain.Core.Interfaces.Tickets
{
    public interface IDetailHistoryRepository
    {
        List<GetTicketDetailHistoryListDto> GetHistoriesIncludeMessage(int ticketDetailId, CancellationToken cancellationToken);
    }
}
