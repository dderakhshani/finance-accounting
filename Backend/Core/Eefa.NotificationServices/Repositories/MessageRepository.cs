using Eefa.NotificationServices.Common.Enum;
using Eefa.NotificationServices.Common.Model;
using Eefa.NotificationServices.Data;
using Eefa.NotificationServices.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.NotificationServices.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageDbContext _messageContext;
        public MessageRepository(MessageDbContext messageDbContext)
        {
            _messageContext=messageDbContext;
        }

        public async Task AddMessage(Message message)
        {
          await  _messageContext.Messages.AddAsync(message);
           await _messageContext.SaveChangesAsync();
        }

        public async Task<List<Message>> GetAllMessages(int userId)
        {
            var messages=await _messageContext.Messages.Where(m=>m.ReceiverUserId==userId).ToListAsync();
            return messages;
        }

        public async Task<List<Message>> GetAllReadMessages(int userId)
        {
            var messages = await _messageContext.Messages.Where(m => m.ReceiverUserId == userId && m.Status==MessageStatus.Read).ToListAsync();
            return messages;
        }

        public async Task MarkMessageAsReadAsync(int messageId)
        {
            var message = await _messageContext.Messages.FirstOrDefaultAsync(x=>x.Id==messageId);
            if (message != null)
            {
                message.Status = MessageStatus.Read;
               _messageContext.Messages.Update(message);
                await _messageContext.SaveChangesAsync();
            }
        }
    }
}
