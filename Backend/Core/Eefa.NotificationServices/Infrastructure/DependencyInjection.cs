using Eefa.NotificationServices.Data;
using Eefa.NotificationServices.Repositories;
using Eefa.NotificationServices.Repositories.Interfaces;
using Eefa.NotificationServices.Services.Interfaces;
using Eefa.NotificationServices.Services.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR;
using Eefa.NotificationServices.Authorization;

namespace Eefa.NotificationServices.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotificationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<MessageDbContext>(options =>
                options.UseSqlServer(configuration["ConnectionStrings:MessageConnectionString"]));

            //jwt
            services.AddJwtAuthentication(configuration);

            // Services         
            services.AddScoped<IUserConnectionManager, UserConnectionManager>();
            services.AddScoped<INotificationService, SignalRService>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            return services;
        }

    }
}
