using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eefa.Admin.Data.Databases.SqlServer.Context;
using Eefa.Persistence.Data.SqlServer;
using Library.ConfigurationAccessor;
using Library.Configurations;
using Library.CurrentUserAccessor;
using Library.Interfaces;
using Library.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using Library.Exceptions.Middleware;
using Library.Common;


namespace Eefa.Admin.WebApi
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


            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.Error = (sender, args) => throw args.ErrorContext.Error)
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

                        //.WithOrigins("http://localhost:4200")
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
                //.AllowCredentials());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Eefa.Admin.WebApi", new OpenApiInfo
                {
                    Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                    Title = "Admin Web API"
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


            services.AddDbContext<AdminUnitOfWork>(options =>
                      options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString().DefaultString));

            services.AddScoped<IRepository, Repository>();

            services.AddScoped<IUnitOfWork, AdminUnitOfWork>();
            services.AddScoped<IAdminUnitOfWork, AdminUnitOfWork>();
            services.AddScoped<IHierarchicalController, HierarchicalController>();
            services.AddScoped<IUpLoader, UpLoader>();

            services.AddScoped<IConfigurationAccessor, ConfigurationAccessor>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddScoped<ITejaratBankServices, TejaratBankServices>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/Eefa.Admin.WebApi/swagger.json", $"Admin Web API {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}"); });

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}

