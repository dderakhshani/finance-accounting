using Eefa.Common;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Infrastructure.Patterns;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Commands.Tickets
{
    public class AddPrivetMessageCommand : IRequest<ServiceResult<int>>
    {
        public int TicketDetailId { get; set; }
        public string Message { get; set; }
    }
    public class AddPrivetMessageCommandHandler : IRequestHandler<AddPrivetMessageCommand, ServiceResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddPrivetMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<int>> Handle(AddPrivetMessageCommand request, CancellationToken cancellationToken)
        {

            PrivetMessage privetMessage = new(request.TicketDetailId, request.Message, null);
            _unitOfWork.PrivetMessageRepository.Add(privetMessage, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<int>.Success(privetMessage.Id);
            //return privetMessage.Id;
        }
    }
}
