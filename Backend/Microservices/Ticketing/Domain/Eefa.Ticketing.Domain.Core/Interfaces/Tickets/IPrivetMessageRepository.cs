using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Domain.Core.Interfaces.Tickets
{
    public interface IPrivetMessageRepository
    {
        Task<List<GetPrivetMessageListDto>> GetPrivetMessageList(int ticketDitailId, CancellationToken cancellationToken);
        Task<GetCreatorUserAndRoleIdDto> GetCreatorUserAndRoleId(int ticketDitailId, CancellationToken cancelationtoken);
    }
}
