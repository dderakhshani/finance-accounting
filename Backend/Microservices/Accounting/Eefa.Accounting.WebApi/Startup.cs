using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Library.ConfigurationAccessor;
using Library.Configurations;
using Library.CurrentUserAccessor;
using Library.Interfaces;
using Library.Utility;
using Microsoft.EntityFrameworkCore;
//using Eefa.Accounting.Application.Hubs;
using Eefa.Accounting.Application;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using Library.Exceptions.Middleware;
using Eefa.Accounting.Infrastructure.Notification;
using Eefa.Accounting.Infrastructure.Notification.Hubs;
using Eefa.NotificationServices.Services.Interfaces;
using Eefa.NotificationServices.Services.SignalR;
using Eefa.NotificationServices.Infrastructure;
using System;

namespace Eefa.Accounting.WebApi
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

            services.AddApplication(_configuration);
            services.AddInfrastructure(_configuration);

            services.AddControllers()
                                   .AddNewtonsoftJson(x => x.SerializerSettings.Error = (sender, args) => throw args.ErrorContext.Error)
                                   .AddNewtonsoftJson(x => x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore)
                                   .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                                   .AddNewtonsoftJson(x => x.SerializerSettings.Converters.Insert(0, new TrimmingStringConverter())
            );

            services.AddMvc();
            services.AddHttpContextAccessor();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Eefa.Accounting.WebApi", new OpenApiInfo
                {
                    Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                    Title = "Accounting Web API"
                });
                c.SchemaFilter<SwaggerExcludeFilter>();


                c.AddSecurityDefinition("Authorization", new()
                {
                    Description = "",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Type = SecuritySchemeType.Http,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme, // or ReferenceType.Parameter
                                Id = "Authorization"
                            }
                        },
                        new string[] {}
                    }
                });
            });



            services.AddDbContext<AccountingUnitOfWork>(options =>
               options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString().DefaultString));
            
            // Remove For Chocolate Factory Project
            services.AddDbContext<DanaAccountingUnitOfWork>(options =>
             options.UseSqlServer(_configuration.GetConnectionString("DanaString")));

            services.AddScoped<IRepository, Eefa.Accounting.Data.Databases.Reposiroty.Repository>();
            services.AddScoped<IUnitOfWork, AccountingUnitOfWork>();
            services.AddScoped<IAccountingUnitOfWork, AccountingUnitOfWork>();
            services.AddScoped<IAccountingUnitOfWorkProcedures, AccountingUnitOfWorkProcedures>();
            services.AddScoped<IHierarchicalController, HierarchicalController>();

            services.AddScoped<IConfigurationAccessor, ConfigurationAccessor>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();         
           services.AddHttpClient<INotificationClient, NotificationClient>();          
        }

        public void Configure(IApplicationBuilder app)
        {

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/Eefa.Accounting.WebApi/swagger.json", $"Accounting Web API {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}"); });

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
               //endpoints.MapHub<AccountingSignalRHub>("/NotificationHub");
               //endpoints.MapHub<UserHub>("/NotificationUserHub");
               
            });

        }
    }
}

