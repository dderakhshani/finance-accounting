using Eefa.Ticketing.Application.Contract.Dtos.Tickets;
using Eefa.Ticketing.Application.Contract.Interfaces.Tickets;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Domain.Core.Enums;
using Eefa.Ticketing.Infrastructure.Patterns;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;


        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(AddTicketDto addTicketDto, int creatorUserId, CancellationToken cancellationToken)
        {
            TicketDetail ticketDetail = new(addTicketDto.Description, addTicketDto.AttachmentIds, addTicketDto.RoleId);
            Ticket ticket = new(addTicketDto.Title, addTicketDto.TopicId, addTicketDto.PageUrl, addTicketDto.RoleId, addTicketDto.Priority, addTicketDto.ReceiverUserId, roleTitle: "fff", ticketDetail);
            _unitOfWork.TicketRepository.Add(ticket, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return ticket.Id;
        }
        public async Task CloseTicket(int ticketId, int creatorUserId, CancellationToken cancellationToken)
        {
            Ticket ticket = await _unitOfWork.TicketRepository.Get(a => a.Id == ticketId, cancellationToken);
            if (ticket == null)
                throw new NullReferenceException("شناسه وارد شده اشتباه است.");
            //var user=getuser();
            if (ticket.CreatedById == 2/*user.id*/)
            {
                await ChangeTicketStatusAsync(ticket, ticketId, TicketStatus.ClosedByRequester, cancellationToken);
            }
            else
            {
                await ChangeTicketStatusAsync(ticket, ticketId, TicketStatus.ClosedByReceiver, cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync();
        }
        //public async Task<List<GetTicketListDto>> GetList(CancellationToken cancellationToken)
        //{
        //    List<GetTicketListDto> tickets = await _unitOfWork.TicketRepository.GetList(cancellationToken);
        //    return tickets;
        //}
        public async Task ChangeTicketStatusAsync(Ticket ticket, int ticketId, TicketStatus ticketStatus, CancellationToken cancellationToken)
        {
            if (ticket is null)
                ticket = await _unitOfWork.TicketRepository.Get(a => a.Id == ticketId, cancellationToken);
            ticket.ChangeStatus(ticketStatus);
            _unitOfWork.TicketRepository.Edit(ticket, cancellationToken);

        }
    }
}
