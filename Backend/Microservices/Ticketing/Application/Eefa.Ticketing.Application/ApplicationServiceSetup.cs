using Eefa.Ticketing.Application.ACL;
using Eefa.Ticketing.Application.Contract.Interfaces.Tickets;
using Eefa.Ticketing.Application.Services.BasicInfos;
using Eefa.Ticketing.Application.Services.Tickets;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Eefa.Ticketing.Application
{
    public static class ApplicationServiceSetup
    {
        public static void AddApplicationServicesSetup(this IServiceCollection services)
        {
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ITicketDetailService, TicketDetailService>();
            services.AddScoped<IPrivetMessageService, PrivetMessageService>();
            services.AddScoped<IDetailHistoryService, DetailHistoryService>();
            services.AddScoped<IIdentity, Identity>();
            services.AddScoped<IBasicInfoService, BasicInfoService>();
            services.AddScoped<IAdmin, Admin>();
        }
    }
}
