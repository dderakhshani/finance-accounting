using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Eefa.Common.Web;
using Eefa.Bursary.Infrastructure;
using Microsoft.AspNetCore.Http;
using Eefa.Common.Common.Attributes;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Application.Behaviors;
using Eefa.Bursary.Application;
using Eefa.Common.CommandQuery.Behaviors;

using Eefa.Common.Common.Abstraction;
using Eefa.Common;
using Quartz;
using SharedCode;


namespace Eefa.Bursary.WebApi
{
    public class Startup : SharedStartup<BursaryContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
          
            services.IncludeMediator(new List<Type>
            {
               typeof(RequestResponseLoggingBehavior<,>),
                typeof(RequestValidationBehavior<,>)
                //typeof(RequestValidationBehavior<,>),
                //typeof(RepositoryBehavior<,>)
            });
            services.AddApplication();

            base.ConfigureServices(services);

            services.AddInfrastructure(_configuration);
        }
    }
}
