using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ServiceStack;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.Hubs
{
    [Authorize]
    public class AccountingSignalRHub : Hub
    {
        public AccountingSignalRHub(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            Mediator = mediator;
            HttpContextAccessor = httpContextAccessor;
        }

        public IMediator Mediator { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        [AllowAnonymous]
        public override async Task OnConnectedAsync()
        {
               
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<dynamic> HandleInvocation (string handlerName, string json)
        {
            try
            {
                Type actionType = GetTypeByName(handlerName);
                var request = JsonConvert.DeserializeObject(json, actionType);
                //var request = Convert.ChangeType(param,actionType);
                return await Mediator.Send(request);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private Type GetTypeByName(string name)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Eefa") || x.FullName.StartsWith("Library")).ToArray())
            {
                var tt = assembly.DefinedTypes.FirstOrDefault(x => x.Name == name);
                if (tt != null)
                {
                    return tt;
                }
            }
            return null;
        }
    }

}



