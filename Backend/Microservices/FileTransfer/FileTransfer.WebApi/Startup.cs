using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using FileTransfer.WebApi.Persistance.Sql;
using Library.ConfigurationAccessor;
using Library.Configurations;
using Library.Interfaces;
using Library.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using FileTransfer.WebApi.Services;

namespace FileTransfer.WebApi
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
            services.AddScoped<IUnitOfWork, FileTransferUnitOfWork>();
            services.AddScoped<IFileTransferUnitOfWork, FileTransferUnitOfWork>();
            services.AddScoped<IArchiveServices, ArchiveServices>();

            services.AddDbContext<FileTransferUnitOfWork>(options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString().DefaultString,opt=>opt.UseNetTopologySuite()), ServiceLifetime.Transient);

            services.IncludeBaseServices();
            services.IncludeCurrentUserAccessor();
            services.IncludeAutoMapper();

            services.IncludeCorsPolicy();
            services.IncludeOAouth();
            services.IncludeSwagger();
            services.AddScoped<IUpLoader, UpLoader>();
            services.IncludeDomainServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AppSettings.LoadConfigurations(new ConfigurationAccessor(_configuration));
            app.IncludeBuffering();
            //app.UseAuditMiddleware();
            //app.UseAuditCustomAction(new CurrentUserAccessor(new HttpContextAccessor()));
            app.IncludeBuffering();
            app.IncludeExceptionMiddleware();
            app.IncludeCorsPolicy();
            app.IncludeSwagger();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, new ConfigurationAccessor(_configuration).GetIoPaths().Root)),
                RequestPath = "/assets"
            });

            app.IncludeRouting();
            app.IncludeAuthentication();
            app.IncludeAuthorization();
            app.IncludeEndpoint();

            app.IncludeRequestResourcesLocalization();
        }
    }
}


