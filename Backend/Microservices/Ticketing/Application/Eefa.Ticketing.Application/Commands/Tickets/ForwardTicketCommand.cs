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
    public class ForwardTicketCommand : IRequest<ServiceResult<int>>
    {
        public int TicketDetailId { get; set; }
        public int SecondaryRoleId { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
    }
    public class ForwardTicketCommandHandler : IRequestHandler<ForwardTicketCommand, ServiceResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketService _ticketService;

        public ForwardTicketCommandHandler(IUnitOfWork unitOfWork, ITicketService ticketService)
        {
            _unitOfWork = unitOfWork;
            _ticketService = ticketService;
        }
        public async Task<ServiceResult<int>> Handle(ForwardTicketCommand request, CancellationToken cancellationToken)
        {
            TicketDetail ticketDetail = await _unitOfWork.TicketDetailRepository.Get(a => a.Id == request.TicketDetailId, cancellationToken);
            if (ticketDetail == null)
                throw new NullReferenceException("شناسه وارد شده اشتباه است.");
            if (ticketDetail.RoleId == request.SecondaryRoleId)
            {
                return ServiceResult<int>.Success(ticketDetail.Id);
            }
            PrivetMessage privetMessage = new(request.UserId, request.Message, null);
            DetailHistory detailHistory = new(ticketDetail.RoleId, request.SecondaryRoleId, privetMessage);
            ticketDetail.AddDetailHistory(detailHistory);
            ticketDetail.AddPrivetMessage(privetMessage);
            ticketDetail.ChangeDepartment(request.SecondaryRoleId);
            _unitOfWork.TicketDetailRepository.Edit(ticketDetail, cancellationToken);
            await _ticketService.ChangeTicketStatusAsync(null, ticketDetail.TicketId, TicketStatus.Forwarded, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<int>.Success(ticketDetail.Id);
        }
    }
}
