using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Contract.Interfaces.Tickets
{
    public interface IPrivetMessageService //: IBaseService<PrivetMessage>
    {
        Task<int> Add(int creatorUserId, string message, int userId, CancellationToken cancellationToken);
        Task<List<GetPrivetMessageListDto>> GetPrivetMessageList(int ticketDitailId, CancellationToken cancellationToken);
    }
}
