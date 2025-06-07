using System;
using System.Collections.Generic;
using Eefa.Contract;
using Eefa.Contract.AdminWorkflow;
using Eefa.Contract.InventoryAccouting;
using Eefa.Persistence.Data.SqlServer;
using Eefa.WorkflowAdmin.WebApi.Application;
using Eefa.WorkflowAdmin.WebApi.Consumers;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Context;
using Library.ConfigurationAccessor;
using Library.Configurations;
using Library.CurrentUserAccessor;
using Library.Interfaces;
using Library.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using static System.Net.Mime.MediaTypeNames;

namespace Eefa.WorkflowAdmin.WebApi
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
            services.IncludeAutoMapper();

            services.AuditService(new ConfigurationAccessor(_configuration));

            services.IncludeResourcesLocalization();

            services.AddScoped<IConfigurationAccessor, ConfigurationAccessor>();
            DependencyInjector.LoadConfigurations(new ConfigurationAccessor(_configuration));
            services.AddScoped<IConfigurationAccessor, ConfigurationAccessor>();

            services.IncludeCurrentUserAccessor();

            services.AddScoped<IUnitOfWork, WorkflowAdminUnitOfWork>();

            //chach
            //services.UseCaching<WorkflowAdminUnitOfWork>(new[]
            //{
            //    typeof(BaseValue), typeof(BaseValueType)
            //});

            // Mock database

            //  services.AddDbContext<WorkflowAdminUnitOfWork>(options => options.UseInMemoryDatabase("MockDB"));

            services.AddDbContext<WorkflowAdminUnitOfWork>(
                options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString().DefaultString));

            services.AddScoped<IHierarchicalController, HierarchicalController>();

            //services.IncludeRedis();

            services.AddScoped<IRepository, Repository>();


            //services.AddScoped<IMongoDataProvider, MongoDataProvider>();
            services.AddScoped<ICorrectionRequestService, CorrectionRequestService>();
            services.IncludeCorsPolicy();
            services.IncludeOAouth();
            services.IncludeSwagger();
            services.IncludeDomainServices();
            services.IncludeBaseServices();

            services.IncludeMessageBusServices(configuration => configuration
                .AddManager(manager => manager.AddLogger(logger => logger.UseSqlite()))
                .AddPrePublishProcessors(new List<Type>() { typeof(MyPreProssessor) })
                .AddHttpRequestHeader(new List<string>() { "Authorization" })
                .AddStaticHeader(new Dictionary<string, string>() { { "messageBusRequest", "true" } })
                .UseRabbitMq(
                    mqConfiguration => mqConfiguration
                        .RabbitMqCredential(new RabbitMqCredential(_configuration["RabbitMqCredentials:RootUri"], _configuration["RabbitMqCredentials:Username"],
                            _configuration["RabbitMqCredentials:Password"]))
                        .Requests(new List<Type>()
                        {
                            typeof(CreateCorrectionRequestEvent),
                            typeof(UpdateVoucherHeadCorrectionRequestCallBackRequestEvent)
                        })
                        .Consumers(new List<Type>
                        {
                            typeof(CreateCorrectionRequestConsumer),
                        })
                        // .UseMessageRetry(2, 100)
                        .RethrowFaultedMessages()
                        .ThrowOnSkippedMessages()
                )
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AppSettings.LoadConfigurations(new ConfigurationAccessor(_configuration));
            app.IncludeBuffering();
            app.UseAuditMiddleware();
            app.UseAuditCustomAction(new CurrentUserAccessor(new HttpContextAccessor()));
            app.IncludeExceptionMiddleware();
            app.IncludeCorsPolicy();
            app.IncludeSwagger();
            app.IncludeRouting();
            app.IncludeAuthentication();
            app.IncludeAuthorization();
            app.IncludeEndpoint();

            app.IncludeRequestResourcesLocalization();
            app.UseMessageBus();
        }
    }
}

