using System;
using System.Collections.Generic;
using Eefa.Common.Validation;
using Eefa.Common.Web;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Aggregates;
using Eefa.Invertory.Infrastructure.Context;
using Eefa.Invertory.Infrastructure.Repositories;
using Eefa.NotificationServices.Infrastructure;
using Eefa.NotificationServices.Services.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eefa.Inventory.WebApi
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
                typeof(RequestValidationBehavior<,>),
                //typeof(RepositoryBehavior<,>)
            });

            services.IncludeBaseServices(_configuration);
            services.IncludeDataServices(typeof(InvertoryContext));
            services.AddDbContext<InvertoryContext>(
                options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString()
                    .DefaultString));

            services.AddScoped<IInvertoryUnitOfWork, InvertoryContext>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(200);
            });
           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StartupConfig.Initialize(new ConfigurationAccessor(_configuration));
            app.IncludeAll();



        }
    }
}

