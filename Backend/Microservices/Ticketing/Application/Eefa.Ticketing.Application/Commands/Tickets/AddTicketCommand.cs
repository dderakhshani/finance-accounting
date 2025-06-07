using Eefa.Common;
using Eefa.NotificationServices.Common.Enum;
using Eefa.NotificationServices.Dto;
using Eefa.NotificationServices.Services.Interfaces;
using Eefa.Ticketing.Application.ACL;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Domain.Core.Enums;
using Eefa.Ticketing.Infrastructure.Patterns;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Commands.Tickets
{
    public class AddTicketCommand : IRequest<ServiceResult<int>>
    {
        public string Title { get; set; }
        public int TopicId { get; set; }
        public string PageUrl { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public Priority Priority { get; set; }
        public int? ReceiverUserId { get; set; }
        public string Description { get; set; }
        public string AttachmentIds { get; set; }
        //public int UserId { get; set; }
        //public int UserRoleId { get; set; }
    }
    public class AddTicketCommandHandler : IRequestHandler<AddTicketCommand, ServiceResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdmin _admin;
        private readonly IIdentity _identity;
        private readonly INotificationClient _notificationClient;
        public AddTicketCommandHandler(IUnitOfWork unitOfWork, IAdmin admin, IIdentity identity, INotificationClient notificationClient)
        {
            _unitOfWork = unitOfWork;
            _admin = admin;
            _identity = identity;
            _notificationClient = notificationClient;
        }
        public async Task<ServiceResult<int>> Handle(AddTicketCommand request, CancellationToken cancellationToken)
        {
            //var loginResult = await _identity.LoginAsync();
            //var role = await _admin.GetRoleById(request.RoleId, loginResult.objResult);
            var role = await _unitOfWork.BaseInfoRepository.GetRoleById(request.RoleId);

            TicketDetail ticketDetail = new(request.Description, request.AttachmentIds, request.RoleId);
            Ticket ticket = new(request.Title, request.TopicId, request.PageUrl, request.RoleId, request.Priority, request.ReceiverUserId, role.Title, ticketDetail);
            _unitOfWork.TicketRepository.Add(ticket, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            await SendNotification(request.RoleId, request.Description);                

            return ServiceResult<int>.Success(ticketDetail.Id);

        }
        private async Task SendNotification(int roleId,string content)
        {           
            var users = await _unitOfWork.BaseInfoRepository.GetUsersByRoleIdAsync(roleId);
            var message = new NotificationDto
            {
                MessageTitle = "تیکت جدید",
                MessageContent = content,
                MessageType = 1,
                Payload = null,
                SendForAllUser = false,
                Status = MessageStatus.Sent,
                OwnerRoleId = 1,
                Listener = "notifyNewTicket"
            };
            foreach (var user in users)
            {
                message.ReceiverUserId = user.Id;
                await _notificationClient.Send(message);
            }
        }
    }
}
