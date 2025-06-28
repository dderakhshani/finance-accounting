using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using Eefa.Common.Data;
using Eefa.Common.Domain;
using Eefa.Common.Validation.Resources;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Eefa.Common.Common.Exceptions;
using System.Threading.Tasks;
using Eefa.NotificationServices.Services.Interfaces;
using Eefa.NotificationServices.Services.SignalR;

namespace Eefa.Common.Web
{
    public static partial class DependencyInjector
    {
        public static IConfigurationAccessor ConfigurationAccessor;

        public static void IncludeBaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IConfigurationAccessor, ConfigurationAccessor>();
            ConfigurationAccessor = new ConfigurationAccessor(configuration);

            services.AddControllers()
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    x.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

                });

            services.AddMvc();
            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped(typeof(IHierarchicalManager<>), typeof(HierarchicalManager<>));
            services.AddScoped<IValidationErrorManager, ValidationErrorManager>();
            services.AddHttpClient<INotificationClient,NotificationClient>();
           
            IncludeOAouth(services);
            IncludeCorsPolicy(services);//CORS must be called before Response Caching
            IncludeMemoryCaching(services);
            IncludeSwagger(services);
            IncludeValidation(services);
          
        }
        public static void IncludeDataServices<T>(this IServiceCollection services, T unitOfWork) where T : Type
        {
            if (!typeof(IUnitOfWork).IsAssignableFrom(unitOfWork))
            {
                throw new Exception("UnitOfWork type parameters must impletement IUnitOfWork");
            }


            AddGenericInterfaceService(services, typeof(IService));
            AddGenericInterfaceService(services, typeof(IQuery));
            AddGenericInterfaceService(services, typeof(IRepository<>));


            //AddGenericInterfaceService(services, typeof(IRepository<>)); will ignore interfaces of Repository<> 
            //So must add this separately
            services.AddScoped(typeof(IRepository<>),
                        typeof(Repository<>));
            services.AddScoped(typeof(ADOConnection<>));
            services.AddScoped(typeof(IUnitOfWork), unitOfWork);

            //services.AddScoped<IUpLoader, UpLoader>();
            //services.AddScoped<ITraverse, Traverse>();
        }

        public static void IncludeMediator(this IServiceCollection services, List<Type> pipelines)
        {
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            foreach (var pipeline in pipelines)
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), pipeline);
            }

            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        }

        public static void IncludeMemoryCaching(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddResponseCaching();
        }

        public static void UseEFCaching<TUnitOfWork>(this IServiceCollection services, Type[] types) where TUnitOfWork : DbContext
        {
            throw new NotImplementedException();
        }

        public static void UsEFeRedisCaching<TUnitOfWork>(this IServiceCollection services, Type[] types) where TUnitOfWork : DbContext
        {

        }




        #region private methods
        static IEnumerable<Type> runtimeTypes;
        private static void AddGenericInterfaceService(IServiceCollection services, Type baseInterface)
        {
            if (runtimeTypes == null)
            {
                runtimeTypes = AppDomain.CurrentDomain.GetAssemblies()
                    //.Where(x => x.FullName != null && !x.FullName.Contains("MongoDB"))
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetExportedTypes());
            }
            var rep = runtimeTypes.FirstOrDefault(x => x.Name == "SalePriceListRepository");
            //Get All Classes that implement the interface which inherit from IService
            //CustomService: ICustomerService (ICustomerService:IService)
            var servicesTypes = runtimeTypes.Where(p =>
                    (baseInterface.IsAssignableFrom(p) || p.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == baseInterface))
                     && p.IsClass && !p.IsAbstract);

            foreach (var type in servicesTypes)
            {
                //Note: class may have other interfaces except ICustomerService
                var interfaces = type.GetInterfaces().Where(i => i != baseInterface
                                      && (baseInterface.IsAssignableFrom(i)
                                      || !(i.IsGenericType && i.GetGenericTypeDefinition() == baseInterface)));
                foreach (var i in interfaces)
                    services.AddScoped(i, type);
            }
        }

        private static void IncludeOAouth(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = ConfigurationAccessor.GetJwtConfiguration().Issuer,
                        ValidAudience = ConfigurationAccessor.GetJwtConfiguration().Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationAccessor.GetJwtConfiguration().Secret)),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"].ToString();

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/accountingHub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken ?? context.Token;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(accessToken))
                                {
                                    context.Token = accessToken ?? context.Token;
                                }
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private static void IncludeCorsPolicy(this IServiceCollection services)
        {
            // allows cross origin access to the resources
            services.AddCors(o => o.AddPolicy(ConfigurationAccessor.GetCorsConfiguration().PolicyName, builder =>
            {
                if (Convert.ToBoolean(ConfigurationAccessor.GetCorsConfiguration().AllowAnyOrigin))
                    builder.AllowAnyOrigin();
                if (Convert.ToBoolean(ConfigurationAccessor.GetCorsConfiguration().AllowAnyMethod))
                    builder.AllowAnyMethod();
                if (Convert.ToBoolean(ConfigurationAccessor.GetCorsConfiguration().AllowAnyHeader))
                    builder.AllowAnyHeader();

            }));
        }

        private static void IncludeSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ConfigurationAccessor.GetSwaggerConfiguration().Name, new OpenApiInfo
                {
                    Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                    Title = ConfigurationAccessor.GetSwaggerConfiguration().Name,
                    //Description = ConfigurationAccessor.GetSwaggerConfiguration().Description,
                    //TermsOfService = new Uri(ConfigurationAccessor.GetSwaggerConfiguration().TermsOfService),
                    //Contact = new OpenApiContact
                    //{
                    //    Name = ConfigurationAccessor.GetSwaggerConfiguration().Contact.Name,
                    //    Email = ConfigurationAccessor.GetSwaggerConfiguration().Contact.Email,
                    //    Url = new Uri(ConfigurationAccessor.GetSwaggerConfiguration().Contact.Url)
                    //}
                });
                c.SchemaFilter<SwaggerExclude>();


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
        }

        private static void IncludeValidation(this IServiceCollection services)
        {
            services.AddScoped<IResourceFactory, ResourcesFactory>();
            services.AddScoped<IValidationFactory, ValidationFactory>();
            services.AddSingleton<ClickRateLimiter>();
        }
        #endregion

    }
}