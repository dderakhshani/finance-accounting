using Eefa.Bursary.Application;
using Eefa.Bursary.Application.Interfaces;
using Eefa.Bursary.Infrastructure.BackgroundTasks;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Bursary.Infrastructure.Repositories;
using Eefa.Common;
using Eefa.Common.Common.Abstraction;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using SharedCode.Contracts.BursaryAccounting;
using System;
using System.Configuration;

namespace Eefa.Bursary.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddQuartz(q =>
            {
                //q.UseMicrosoftDependencyInjectionScopedJobFactory();

                //var zafarJobKey = new JobKey("TransactionZafarBankBackgroundJob");
                //q.AddJob<TransactionZafarBankBackgroundJob>(opts => opts.WithIdentity(zafarJobKey));
                //q.AddTrigger(opts => opts
                //    .ForJob(zafarJobKey)
                //    .WithIdentity("ZafarBankTrigger")
                //    .WithSimpleSchedule(x => x
                //        .WithIntervalInMinutes(2)
                //        .RepeatForever()));




                //var ardakanJobKey = new JobKey("TransactionArdakanBankBackgroundJob");
                //q.AddJob<TransactionArdakanBankBackgroundJob>(opts => opts.WithIdentity(ardakanJobKey));
                //q.AddTrigger(opts => opts
                //    .ForJob(ardakanJobKey)
                //    .WithIdentity("ArdakanBankTrigger")
                //    .WithSimpleSchedule(x => x
                //        .WithIntervalInMinutes(6)
                //        .RepeatForever()));

                //var outboxToAccountingJobKey = new JobKey("OutboxToAccountingBackgroundJob");
                //q.AddJob<OutboxProcessorToAccountingJob>(opts => opts.WithIdentity(outboxToAccountingJobKey));
                //q.AddTrigger(opts => opts
                //    .ForJob(outboxToAccountingJobKey)
                //    .WithIdentity("OutboxToAccountingTrigger")
                //    .WithSimpleSchedule(x => x
                //        .WithIntervalInMinutes(4)
                //        .RepeatForever()));



                //var outboxToBursaryJobKey = new JobKey("OutboxToBursaryBackgroundJob");
                //q.AddJob<OutboxProcessorToBursuryJob>(opts => opts.WithIdentity(outboxToBursaryJobKey));
                //q.AddTrigger(opts => opts
                //    .ForJob(outboxToBursaryJobKey)
                //    .WithIdentity("OutboxToBursaryTrigger")
                //    .WithSimpleSchedule(x => x
                //        .WithIntervalInMinutes(4)
                //        .RepeatForever()));


            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            services.AddDbContext<OutboxDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("OutboxDb")));

            services.AddTransient<IBursaryUnitOfWork, BursaryContext>();
            services.AddTransient<IApplicationLogs, ApplicationLogs>();
            services.AddTransient<ITejaratBankServices, TejaratBankServices>();
            services.AddTransient<IOutboxRepository, OutboxRepository>();
            services.AddScoped<ITransactionBankServices, TransactionBankServices>();

            services.AddMassTransit(cfg =>
            {
                cfg.AddRequestClient<FinancialEvent>();

                cfg.UsingRabbitMq((context, rabbitCfg) =>
                {
                    rabbitCfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
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
                logging.SetMinimumLevel(LogLevel.Error);   
            });

        }
    }
}
