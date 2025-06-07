using System;
using System.Collections.Generic;
using Eefa.Common.Validation;
using Eefa.Common.Web;
using Eefa.Purchase.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eefa.Purchase.WebApi
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

          
            services.AddDbContext<PurchaseContext>(
                options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString()
                    .DefaultString));

            services.IncludeDataServices(typeof(PurchaseContext));
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

