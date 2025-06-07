using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eefa.Audit.Data.Databases.Entities;
using Eefa.Audit.Data.Databases.SqlServer.Context;
using Eefa.Persistence.Data.SqlServer;
using Eefa.Persistence.DataConfiguration;
using Library.ConfigurationAccessor;
using Library.Configurations;
using Library.Interfaces;
using Library.Utility;

namespace Eefa.Audit.WebApi
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

            services.IncludeResourcesLocalization();

            services.AddScoped<IConfigurationAccessor, ConfigurationAccessor>();
            DependencyInjector.LoadConfigurations(new ConfigurationAccessor(_configuration));
            services.IncludeBaseServices();
            services.AddScoped<IConfigurationAccessor, ConfigurationAccessor>();

            services.IncludeCurrentUserAccessor();

            services.AddScoped<IUnitOfWork, AuditUnitOfWork>();

            //chach
            services.UseCaching<AuditUnitOfWork>(new[]
            {
                typeof(BaseValue), typeof(BaseValueType)
            });

            // Mock database

            // services.AddDbContext<AdminUnitOfWork>(options => options.UseInMemoryDatabase("MockDB"));

            //services.AddDbContext<AdminUnitOfWork>(options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString().DefaultString,opt=>opt.UseNetTopologySuite()), ServiceLifetime.Transient);

            services.AddScoped<IHierarchicalController, HierarchicalController>();

            services.IncludeRedis();

            services.AddScoped<IRepository, Repository>();

            //services.IncludeMediator(new List<Type>
            //{
            //    typeof(RequestValidationBehavior<,>),
            //    typeof(RepositoryBehavior<,>)
            //});

             services.IncludeAutoMapper();

            //services.AddScoped<IMongoDataProvider, MongoDataProvider>(o => new MongoDataProvider(configurator => configurator
            //    .ConnectionString(_configuration["AuditMongoDb:ConnectionString"])
            //    .Database(_configuration["AuditMongoDb:Database"])
            //    .Collection(_configuration["AuditMongoDb:Collection"])
            //    .SerializeAsBson(Convert.ToBoolean(_configuration["AuditMongoDb:UseBson"]))));

            //services.AddScoped<IAuditMontiroringRepository, AuditMonitoringRepository>();

            services.IncludeCorsPolicy();
            services.IncludeOAouth();
            services.IncludeSwagger();
            services.IncludeDomainServices();

           // Application.ValidationLocator.ValidationFunctionsLocator.Set();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AppSettings.LoadConfigurations(new ConfigurationAccessor(_configuration));
            app.IncludeBuffering();
           // app.UseAuditMiddleware();
           // app.UseAuditCustomAction(new CurrentUserAccessor(new HttpContextAccessor()));
            app.IncludeBuffering();
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

