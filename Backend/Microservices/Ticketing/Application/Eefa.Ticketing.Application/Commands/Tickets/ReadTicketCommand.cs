using Eefa.Common;
using Eefa.Ticketing.Application.Contract.Interfaces.Tickets;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Domain.Core.Enums;
using Eefa.Ticketing.Infrastructure.Patterns;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Commands.Tickets
{
    public class ReadTicketCommand : IRequest<ServiceResult<int>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
    public class ReadTicketCommandHandler : IRequestHandler<ReadTicketCommand, ServiceResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketService _ticketService;

        public ReadTicketCommandHandler(IUnitOfWork unitOfWork, ITicketService ticketService)
        {
            _unitOfWork = unitOfWork;
            _ticketService = ticketService;
        }
        public async Task<ServiceResult<int>> Handle(ReadTicketCommand request, CancellationToken cancellationToken)
        {
            TicketDetail ticketDetail = await _unitOfWork.TicketDetailRepository.Get(a => a.Id == request.Id, cancellationToken);
            if (ticketDetail == null)
                throw new NullReferenceException("شناسه وارد شده اشتباه است.");

            ticketDetail.ReadTicket(request.UserId);
            _unitOfWork.TicketDetailRepository.Edit(ticketDetail, cancellationToken);

            await _ticketService.ChangeTicketStatusAsync(null, ticketDetail.TicketId, TicketStatus.Ongoing, cancellationToken);

            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<int>.Success(ticketDetail.Id);
        }
    }
}
