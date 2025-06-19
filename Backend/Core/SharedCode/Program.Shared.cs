using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eefa.Common;
using Eefa.Common.Web;
using Eefa.Common.Common.Attributes;
using Eefa.Common.Common.Abstraction;
using Eefa.Common.CommandQuery.Behaviors;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

using NLog;
using NLog.Web;
using Eefa.Common.Domain;
using System.Configuration;


namespace SharedCode
{
    public class SharedProgram
    {
        public static void SetupRun<TContext>(WebApplicationBuilder builder, IConfiguration configuration) where TContext : DbContext
        {
            builder.Configuration.AddJsonFile("appsettings-shared.json");
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddJsonFile($"appsettings-shared.{builder.Environment.EnvironmentName}.json");
            }

            builder.Services.AddDbContext<TContext>(options => options.UseSqlServer(new ConfigurationAccessor(configuration).GetConnectionString().DefaultString));
            builder.Services.IncludeBaseServices(configuration);
            builder.Services.IncludeDataServices(typeof(TContext));

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = Context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            builder.Services.AddMemoryCache();

            builder.Services.AddScoped<ClickRateLimiterAttribute>(sp =>
            {
                int allowedClickIntervalSeconds = 3;
                int allowedClickCount = 3;
                return new ClickRateLimiterAttribute(allowedClickIntervalSeconds, allowedClickCount);
            });

            var app = builder.Build();

            StartupConfig.Initialize(new ConfigurationAccessor(configuration));

            app.IncludeAll();

            app.Run();
        }

        public static void SetupRunOld<TContext, TStartup>(string[] args)
            where TContext : DbContext
            where TStartup : SharedStartup<TContext>
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                CreateHostBuilder<TContext, TStartup>(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder<TContext, TStartup>(string[] args)
            where TContext : DbContext
            where TStartup : SharedStartup<TContext>
            =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                   webBuilder.UseStartup<TStartup>();
                   webBuilder.ConfigureAppConfiguration((context, config) =>
                   {
                       config.AddJsonFile($"appsettings-shared.json");
                       if (context.HostingEnvironment.IsDevelopment())
                           config.AddJsonFile($"appsettings-shared.{env}.json");
                          
                   });
               })
               .ConfigureLogging(clog =>
               {
                   clog.ClearProviders();
               })
           .UseNLog();
    }

    public class SharedStartup<TContext> where TContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public SharedStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TContext>(options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString().DefaultString));
            services.IncludeBaseServices(_configuration);
            services.IncludeDataServices(typeof(TContext));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = Context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMemoryCache();

            services.AddScoped<ClickRateLimiterAttribute>(sp =>
            {
                int allowedClickIntervalSeconds = 3;
                int allowedClickCount = 3;
                return new ClickRateLimiterAttribute(allowedClickIntervalSeconds, allowedClickCount);
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
