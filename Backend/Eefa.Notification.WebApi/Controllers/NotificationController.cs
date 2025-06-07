using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Eefa.NotificationServices.Common.Model;
using Eefa.NotificationServices.Services.Interfaces;
using Eefa.NotificationServices.Services.SignalR;
using Eefa.NotificationServices.Dto;

namespace Eefa.Notification.WebApi
{   
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NotificationController : ControllerBase
    {
        private INotificationService _notificationService;     
        public NotificationController(INotificationService notificationService)
        {
           _notificationService = notificationService;
        }
        [HttpPost]
        public async Task<IActionResult> Send([FromBody] NotificationDto message)
        {            
            await _notificationService.SendNotification(message);
            return Ok();
        }

    }
}