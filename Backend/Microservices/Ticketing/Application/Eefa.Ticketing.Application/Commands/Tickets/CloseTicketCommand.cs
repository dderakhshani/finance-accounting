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
    public class CloseTicketCommand : IRequest<ServiceResult<int>>
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }
    }
    public class CloseTicketCommandHandler : IRequestHandler<CloseTicketCommand, ServiceResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketService _ticketService;

        public CloseTicketCommandHandler(IUnitOfWork unitOfWork, ITicketService ticketService)
        {
            _unitOfWork = unitOfWork;
            _ticketService = ticketService;
        }

        public async Task<ServiceResult<int>> Handle(CloseTicketCommand request, CancellationToken cancellationToken)
        {
            Ticket ticket = await _unitOfWork.TicketRepository.Get(a => a.Id == request.TicketId, cancellationToken);
            if (ticket == null)
                throw new NullReferenceException("شناسه وارد شده اشتباه است.");
            //var user=getuser();
            if (ticket.CreatedById == request.UserId)
            {
                await _ticketService.ChangeTicketStatusAsync(ticket, request.TicketId, TicketStatus.ClosedByRequester, cancellationToken);
            }
            else
            {
                await _ticketService.ChangeTicketStatusAsync(ticket, request.TicketId, TicketStatus.ClosedByReceiver, cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<int>.Success(ticket.Id);
        }
    }
}
