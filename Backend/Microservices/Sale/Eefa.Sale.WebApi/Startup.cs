using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Eefa.Common.Web;
using Eefa.Sale.Application.Common.Interfaces;
using Eefa.Sale.Infrastructure.Context;
using Eefa.Sale.Infrastructure.CrmServices;


namespace Eefa.Sale.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.IncludeMediator(new List<Type>
            {
                //typeof(RequestValidationBehavior<,>),
                //typeof(RepositoryBehavior<,>)
            });

            services.IncludeBaseServices(_configuration);
            services.IncludeDataServices(typeof(SalesUnitOfWork));

            services.AddDbContext<SalesUnitOfWork>(
                options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString()
                    .DefaultString));

            services.AddScoped<ISalesUnitOfWork, SalesUnitOfWork>();
            services.AddScoped<ICrmServices,CrmServices>();

            //services.IncludeMessageBusServices(configuration => configuration
            //    .AddManager(manager => manager.AddLogger(logger => logger.UseSqlite()))
            //    .AddPrePublishProcessors(new List<Type>() { typeof(MyPreProssessor) })
            //    .AddHttpRequestHeader(new List<string>() { "Authorization" })
            //    .AddStaticHeader(new Dictionary<string, string>() { { "messageBusRequest", "true" } })
            //    .UseRabbitMq(
            //        mqConfiguration => mqConfiguration
            //            .RabbitMqCredential(new RabbitMqCredential(_configuration["RabbitMqCredentials:RootUri"], _configuration["RabbitMqCredentials:Username"],
            //                _configuration["RabbitMqCredentials:Password"]))
            //            .Requests(new List<Type>()
            //            {
            //                typeof(CreateCustomerPersonEventRequest)
            //            })
            //            .Consumers(new List<Type>
            //            {
            //            })
            //            // .UseMessageRetry(2, 100)
            //            .RethrowFaultedMessages()
            //            .ThrowOnSkippedMessages()
            //    )
            //);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StartupConfig.Initialize(new ConfigurationAccessor(_configuration));
            app.IncludeAll();
            //app.UseMessageBus();

        }
    }
}

