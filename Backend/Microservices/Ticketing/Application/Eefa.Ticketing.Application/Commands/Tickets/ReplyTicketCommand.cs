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
    public class ReplyTicketCommand : IRequest<ServiceResult<int>>
    {
        public int TicketId { get; set; }
        public string Description { get; set; }
        public string AttachmentIds { get; set; }
        //public int UserId { get; set; }
        //public int UserRoleId { get; set; }
    }
    public class ReplyTicketCommandHandler : IRequestHandler<ReplyTicketCommand, ServiceResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketService _ticketService;

        public ReplyTicketCommandHandler(IUnitOfWork unitOfWork, ITicketService ticketService)
        {
            _unitOfWork = unitOfWork;
            _ticketService = ticketService;
        }
        public async Task<ServiceResult<int>> Handle(ReplyTicketCommand request, CancellationToken cancellationToken)
        {
            Ticket ticket = await _unitOfWork.TicketRepository.Get(a => a.Id == request.TicketId, cancellationToken);
            if (ticket == null)
                throw new NullReferenceException("شناسه وارد شده اشتباه است.");
            TicketDetail ticketDetail = new(request.TicketId, request.Description, request.AttachmentIds, ticket.RoleId);
            _unitOfWork.TicketDetailRepository.Add(ticketDetail, cancellationToken);

            if (ticket.CreatedById == ticketDetail.CreatedById)
            {
                await _ticketService.ChangeTicketStatusAsync(ticket, ticket.Id, TicketStatus.RequesterAnswered, cancellationToken);
            }
            else
            {
                await _ticketService.ChangeTicketStatusAsync(ticket, ticket.Id, TicketStatus.ReceiverAnswered, cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<int>.Success(ticketDetail.Id);
        }
    }
}
