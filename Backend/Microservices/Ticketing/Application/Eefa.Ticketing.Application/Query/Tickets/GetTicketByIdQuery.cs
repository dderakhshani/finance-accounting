using Eefa.Common;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Infrastructure.Patterns;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Query.Tickets
{
    public class GetTicketByIdQuery : IRequest<ServiceResult<Ticket>>
    {
        public int TicketId { get; set; }
    }
    public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, ServiceResult<Ticket>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTicketByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<Ticket>> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
        {
            Ticket result = await _unitOfWork.TicketRepository.Get(a => a.Id == request.TicketId, cancellationToken);
            return ServiceResult<Ticket>.Success(result);
        }
    }
}
