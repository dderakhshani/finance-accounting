using Eefa.NotificationServices.Common.Model;
using Eefa.NotificationServices.Dto;
using System.Threading.Tasks;

namespace Eefa.NotificationServices.Services.Interfaces
{
    public interface INotificationClient
    {
         Task Send(NotificationDto message);
    }
}
