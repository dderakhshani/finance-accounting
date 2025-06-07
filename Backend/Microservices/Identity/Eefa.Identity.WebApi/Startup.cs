using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eefa.Identity.Data.Databases.Entities;
using Eefa.Identity.Data.Databases.SqlServer.Context;
using Eefa.Identity.Services.Interfaces;
using Eefa.Identity.Services.UserSetting;
using Eefa.Persistence.Data.SqlServer;
using Library.ConfigurationAccessor;
using Library.Configurations;
using Library.Exceptions.Middleware;
using Library.Interfaces;
using Library.Utility;

namespace Eefa.Identity.WebApi
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
            //services.AuditService(new ConfigurationAccessor(_configuration));

            services.AddScoped<IConfigurationAccessor, ConfigurationAccessor>();
            DependencyInjector.LoadConfigurations(new ConfigurationAccessor(_configuration));

            services.IncludeResourcesLocalization();

            services.IncludeBaseServices();

            services.IncludeCurrentUserAccessor();

            services.AddScoped<IUnitOfWork, IdentityUnitOfWork>();


            services.UseCaching<IdentityUnitOfWork>(new[]
            {
                typeof(BaseValue), typeof(BaseValueType), typeof(UserSetting)
            });

            services.AddScoped<IHierarchicalController, HierarchicalController>();

            //services.IncludeRedis();

            //services.AddDbContext<IdentityUnitOfWork>(options =>
            //    options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString().DefaultString));
            services.AddScoped<IRepository, Repository>();


            //services.IncludeRedis<RedisDataProvider>();
            services.IncludeAutoMapper();

            //services.AddScoped<IMongoDataProvider, MongoDataProvider>();
            //services.AddScoped<IAuditMontiroringRepository, AuditMonitoringRepository>();
            services.AddScoped<IUserSettingsAccessor, UserSettingsAccessor>();

            services.IncludeCorsPolicy();
            services.IncludeOAouth();
            services.IncludeSwagger();
            services.IncludeDomainServices();
        }

        public void Configure(IApplicationBuilder app)
        {
            AppSettings.LoadConfigurations(new ConfigurationAccessor(_configuration));
            app.IncludeBuffering();
            //app.UseAuditMiddleware();
            //app.UseAuditCustomAction(new CurrentUserAccessor(new HttpContextAccessor()));
            app.UseMiddleware<AuthenticationMiddleware>();
            app.IncludeExceptionMiddleware();
            app.IncludeCorsPolicy();
            app.IncludeSwagger();
            app.IncludeRouting();
            app.IncludeAuthentication();
            app.IncludeAuthorization();
            app.IncludeEndpoint();
            app.IncludeRequestResourcesLocalization();
        }
    }
}
