using Eefa.Ticketing.Application.Contract.Interfaces.Tickets;
using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Infrastructure.Patterns;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Services.Tickets
{
    public class PrivetMessageService : IPrivetMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrivetMessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(int ticketDetailId, string message, int userId, CancellationToken cancellationToken)
        {
            PrivetMessage privetMessage = new(ticketDetailId, message, null);
            _unitOfWork.PrivetMessageRepository.Add(privetMessage, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return privetMessage.Id;
        }

        public async Task<List<GetPrivetMessageListDto>> GetPrivetMessageList(int ticketDitailId, CancellationToken cancellationToken)
        {
            List<GetPrivetMessageListDto> result = await _unitOfWork.PrivetMessageRepository.GetPrivetMessageList(ticketDitailId, cancellationToken);
            return result;
        }
    }
}
