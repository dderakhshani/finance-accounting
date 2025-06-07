using Eefa.NotificationServices.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.NotificationServices.Services.SignalR
{
    public class UserHub : Hub
    {        
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserConnectionManager _userConnectionManager;
        public UserHub(IUserConnectionManager userConnectionManager, IHttpContextAccessor httpContextAccessor)//, IUserConnectionManager1 userConnectionManager1
        {           
            _httpContextAccessor = httpContextAccessor;
            _userConnectionManager = userConnectionManager;
        }     

        public override async Task OnConnectedAsync()
        {
            var userId = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "id")?.Value; // Assume UserIdentifier is set
            if (userId == null)
                return;

            var connectionId = Context.ConnectionId;
            _userConnectionManager.AddUserConnection(userId, connectionId);
            await base.OnConnectedAsync();
        }

        //Called when a connection with the hub is terminated.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);
        }
    }
}

