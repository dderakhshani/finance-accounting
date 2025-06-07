using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Eefa.Common.Web;
using Eefa.Commodity.Data.Context;

namespace Eefa.Commodity.WebApi
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
            services.AddScoped<ICommodityUnitOfWork, CommodityUnitOfWork>();
            services.IncludeBaseServices(_configuration);
            services.IncludeDataServices(typeof(CommodityUnitOfWork));

            services.AddDbContext<CommodityUnitOfWork>(
                options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString()
                    .DefaultString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StartupConfig.Initialize(new ConfigurationAccessor(_configuration));
            app.IncludeAll();

        }
    }
}



