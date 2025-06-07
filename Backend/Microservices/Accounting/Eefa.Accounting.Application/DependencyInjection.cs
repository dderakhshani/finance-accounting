using Eefa.Accounting.Application.Common.Behaviors;
using Eefa.Accounting.Application.Messages;
using Eefa.Accounting.Application.Services.EventManager;
using Eefa.Accounting.Application.Services.Logs;
using Eefa.Accounting.Application.UseCases.VouchersHead.Services;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Net.Pkcs11Interop.Common;
using SharedCode.Contracts.BursaryAccounting;
using System;
using System.Reflection;

namespace Eefa.Accounting.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ApplicationRequestsLogBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ApplicationEventsPersistenceBehavior<,>));
                       
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddSingleton<IVoucherHeadCacheServices,VoucherHeadCacheServices>();

            services.AddScoped<IApplicationEventsManager, ApplicationEventsManager>();
            services.AddScoped<IApplicationRequestLogManager, ApplicationRequestLogManager>();

            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<FinancialEventConsumer>();

                cfg.UsingRabbitMq((context, rabbitCfg) =>
                {
                    rabbitCfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    rabbitCfg.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(5));
                    });

                    rabbitCfg.Message<FinancialEvent>(m =>
                    {
                        m.SetEntityName("financial-events-exchange");
                    });
                    rabbitCfg.Publish<FinancialEvent>(p =>
                    {
                        p.ExchangeType = "direct";
                        p.Durable = true;
                    });

                    rabbitCfg.ConfigureEndpoints(context);
                });
            });


            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);  // سطح لاگ به Debug تغییر پیدا کند
            });


            return services;
        }
    }
}
