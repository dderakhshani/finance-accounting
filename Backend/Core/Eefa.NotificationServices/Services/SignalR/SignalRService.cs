using Eefa.NotificationServices.Common.Model;
using Eefa.NotificationServices.Data;
using Eefa.NotificationServices.Dto;
using Eefa.NotificationServices.Repositories.Interfaces;
using Eefa.NotificationServices.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Eefa.NotificationServices.Services.SignalR
{
    public class SignalRService : INotificationService
    {
        private readonly IHubContext<UserHub> _userHubContext;
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly IMessageRepository _messageRepository;
        public SignalRService(IHubContext<UserHub> userHubContext, IUserConnectionManager userConnectionManager,
            IMessageRepository messageRepository)
        {
            _userHubContext = userHubContext??throw new ArgumentNullException(nameof(userHubContext));           
            _messageRepository = messageRepository;
            _userConnectionManager = userConnectionManager;
        }

        public async Task SendNotification(NotificationDto messageDto)
        {
            var message = new Message
            {
                MessageContent = messageDto.MessageContent,
                MessageTitle = messageDto.MessageTitle,
                CreatedAt = DateTime.UtcNow,
                CreatedById = 1,
                MessageType = messageDto.MessageType,
                OwnerRoleId = messageDto.OwnerRoleId,
                ReceiverUserId = messageDto.ReceiverUserId,
                SendForAllUser = messageDto.SendForAllUser,
                Status = messageDto.Status,
                Payload = messageDto.Payload,
                IsDeleted=false
            };
            await _messageRepository.AddMessage(message);
            var connections = _userConnectionManager.GetUserConnections(message.ReceiverUserId.ToString());
            foreach (var connectionId in connections)
            {
                await _userHubContext.Clients.Client(connectionId).SendAsync(messageDto.Listener, message);
            }
        }


    }
}
