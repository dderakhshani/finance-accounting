using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Infrastructure.Notification
{
    public interface IUserConnectionManager
    {
        void KeepUserConnection(string userId, string connectionId);
        void RemoveUserConnection(string connectionId);
        List<string> GetUserConnections(string userId);
    }
}
