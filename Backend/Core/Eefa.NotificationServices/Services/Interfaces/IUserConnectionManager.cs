using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.NotificationServices.Services.Interfaces
{
    public interface IUserConnectionManager
    {
        void AddUserConnection(string userId, string connectionId);
        void RemoveUserConnection(string connectionId);
        List<string> GetUserConnections(string userId);      
    }
}
