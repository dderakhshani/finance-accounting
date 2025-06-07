using System;
using System.Collections.Generic;
using Eefa.Common.Validation;
using Eefa.Common.Web;
using Eefa.Logistic.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eefa.Logistic.WebApi
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

          
            services.AddDbContext<LogisticContext>(
                options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString()
                    .DefaultString));

            services.AddDbContext<SamaTowzinContext>(
                options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetSamaConnectionString()
                    .DefaultString));

            services.IncludeDataServices(typeof(LogisticContext));
            services.IncludeBaseServices(_configuration);
           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StartupConfig.Initialize(new ConfigurationAccessor(_configuration));
            app.IncludeAll();

        }
    }
}

