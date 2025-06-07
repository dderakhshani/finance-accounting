using Eefa.NotificationServices.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.NotificationServices.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task AddMessage(Message message);
        Task MarkMessageAsReadAsync(int id);
        Task<List<Message>> GetAllMessages(int userId);
        Task<List<Message>> GetAllReadMessages(int userId);

    }
}
