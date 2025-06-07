using Eefa.Ticketing.Application.Contract.Dtos.Tickets;
using Eefa.Ticketing.Application.Contract.Interfaces.Tickets;
using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Domain.Core.Enums;
using Eefa.Ticketing.Infrastructure.Patterns;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Services.Tickets
{
    public class TicketDetailService : ITicketDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketService _ticketService;

        public TicketDetailService(IUnitOfWork unitOfWork, ITicketService ticketService)
        {
            _unitOfWork = unitOfWork;
            _ticketService = ticketService;
        }

        public async Task<int> ReadTicket(int id, int userId, CancellationToken cancellationToken)
        {
            TicketDetail ticketDetail = await _unitOfWork.TicketDetailRepository.Get(a => a.Id == id, cancellationToken);
            if (ticketDetail == null)
                throw new NullReferenceException("شناسه وارد شده اشتباه است.");

            ticketDetail.ReadTicket(userId);
            _unitOfWork.TicketDetailRepository.Edit(ticketDetail, cancellationToken);

            await _ticketService.ChangeTicketStatusAsync(null, ticketDetail.TicketId, TicketStatus.Ongoing, cancellationToken);

            await _unitOfWork.SaveChangesAsync();
            return ticketDetail.Id;
        }
        public async Task<int> ReplyTicket(ReplyTicketDto replyTicketDto, int userId, CancellationToken cancellationToken)
        {
            Ticket ticket = await _unitOfWork.TicketRepository.Get(a => a.Id == replyTicketDto.TicketId, cancellationToken);
            if (ticket == null)
                throw new NullReferenceException("شناسه وارد شده اشتباه است.");
            TicketDetail ticketDetail = new(replyTicketDto.TicketId, replyTicketDto.Description, replyTicketDto.AttachmentIds, ticket.RoleId);
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
            return ticketDetail.Id;
        }
        public async Task<int> ForwardTicketAsync(ForwardTicketDto forwardTicketDto, int userId, CancellationToken cancellationToken)
        {
            TicketDetail ticketDetail = await _unitOfWork.TicketDetailRepository.Get(a => a.Id == forwardTicketDto.TicketDetailId, cancellationToken);
            if (ticketDetail == null)
                throw new NullReferenceException("شناسه وارد شده اشتباه است.");
            if (ticketDetail.RoleId == forwardTicketDto.SecondaryRoleId)
            {
                return ticketDetail.Id;
            }
            PrivetMessage privetMessage = new(userId, forwardTicketDto.Message, null);
            DetailHistory detailHistory = new(ticketDetail.RoleId, forwardTicketDto.SecondaryRoleId, privetMessage);
            ticketDetail.AddDetailHistory(detailHistory);
            ticketDetail.AddPrivetMessage(privetMessage);
            ticketDetail.ChangeDepartment(forwardTicketDto.SecondaryRoleId);
            _unitOfWork.TicketDetailRepository.Edit(ticketDetail, cancellationToken);
            await _ticketService.ChangeTicketStatusAsync(null, ticketDetail.TicketId, TicketStatus.Forwarded, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return ticketDetail.Id;
        }

        public async Task<List<GetTicketDetailListDto>> GetList(int ticketId, int userId, CancellationToken cancellationToken)
        {
            List<TicketDetail> unreadedList = await _unitOfWork.TicketDetailRepository.GetUnreadListAsync(ticketId, cancellationToken);
            foreach (var item in unreadedList)
            {
                item.ReadTicket(userId);
                _unitOfWork.TicketDetailRepository.Edit(item, cancellationToken);
                await _ticketService.ChangeTicketStatusAsync(null, item.TicketId, TicketStatus.Ongoing, cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync();
            return await _unitOfWork.TicketDetailRepository.GetList(ticketId, cancellationToken);
        }
    }
}
