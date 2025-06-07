using Eefa.Ticketing.Domain.Core.Interfaces.BaseInfo;
using Eefa.Ticketing.Domain.Core.Interfaces.Tickets;
using Eefa.Ticketing.Infrastructure.Database.Context;
using Eefa.Ticketing.Infrastructure.Patterns;
using Eefa.Ticketing.Infrastructure.Repositories.BaseInfo;
using Eefa.Ticketing.Infrastructure.Repositories.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Eefa.Ticketing.Infrastructure
{
    public static class InfrastructureSetup
    {
        public static void AddInfrastructureSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddDbContext<EefaTicketingContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultString")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketDetailRepository, TicketDetailRepository>();
            services.AddScoped<IDetailHistoryRepository, DetailHistoryRepository>();
            services.AddScoped<IPrivetMessageRepository, PrivetMessageRepository>();
            services.AddScoped<IBaseInfoRepository, BaseInfoRepository>();
        }
    }
}
