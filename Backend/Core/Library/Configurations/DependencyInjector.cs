using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Resources;
using Library.Utility;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Library.Configurations
{
    public static partial class DependencyInjector
    {
        public static IConfigurationAccessor ConfigurationAccessor;
        public static void LoadConfigurations(IConfigurationAccessor configurationAccessor)
        {
            ConfigurationAccessor = configurationAccessor;
        }


        public static void IncludeBaseServices(this IServiceCollection services)
        {

            services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.Error = (sender, args) => throw args.ErrorContext.Error)
                .AddNewtonsoftJson(x => x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore)
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(x => x.SerializerSettings.Converters.Insert(0, new TrimmingStringConverter())



                );

            services.AddMvc();
            services.AddHttpContextAccessor();
        }


        public static void IncludeResourcesLocalization(this IServiceCollection services)
        {
            //services.AddLocalization(options => options.ResourcesPath = "Resources/ValidationMsg");
            //services.AddScoped<IStringLocalizer, StringLocalizer<ValidationMsg>>();
            //services
            //    .Configure<RequestLocalizationOptions>(options =>
            //    {
            //        var cultures = new[]
            //        {
            //            new CultureInfo("en"),
            //            new CultureInfo("fa")
            //        };
            //        options.DefaultRequestCulture = new RequestCulture(culture: "fa-IR", uiCulture: "fa-IR");
            //        options.SetDefaultCulture("fa-IR");
            //        options.SupportedCultures = cultures;
            //        options.SupportedUICultures = cultures;
            //    });
            //services.AddScoped<IResourcesFactory, ValidationFactory<ValidationMsg>>();
        }

        public static void IncludeCurrentUserAccessor(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor.CurrentUserAccessor>();
        }


        public static void IncludeDomainServices(this IServiceCollection services)
        {

            var servicesTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => (x.FullName.StartsWith("Eefa") || x.FullName.StartsWith("Library")) && x.FullName.Contains("MongoDB") == false)
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IService).IsAssignableFrom(p) && p.IsClass &&
                            p.GetInterfaces().Any(x => x.Name == "I" + p.Name));

            foreach (var type in servicesTypes)
            {
                services.AddScoped(type.GetInterfaces().FirstOrDefault(i => i.Name == "I" + type.Name)!, type);
            }

            services.AddScoped<IUpLoader, UpLoader>();
            //  services.AddScoped<ITraverse, Traverse>();
            services.AddScoped<IResourcesFactory, ResourcesFactory>();
            services.AddScoped<IResourceFactory, ValidationFactory>();
            services.AddScoped<IMetaDataFactory, MetaDataFactory>();
            services.AddScoped<IEntityUtill, EntityUtill>();
        }

        public static void IncludeMediator(this IServiceCollection services, List<Type> pipelines)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Eefa") || x.FullName.StartsWith("Library")).ToArray();
            services.AddMediatR(assemblies);
            foreach (var pipeline in pipelines)
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), pipeline);
            }


        }

        public static void IncludeAutoMapper(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Eefa") || x.FullName.StartsWith("Library")).ToArray();
            services.AddAutoMapper(assemblies);
        }

        public static void IncludeOAouth(this IServiceCollection services)
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

        public static void IncludeCorsPolicy(this IServiceCollection services)
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

        public static void IncludeSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ConfigurationAccessor.GetSwaggerConfiguration().Name, new OpenApiInfo
                {
                    Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                    Title = ConfigurationAccessor.GetSwaggerConfiguration().Name
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

                var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void UseCaching<TUnitOfWork>(this IServiceCollection services, Type[] types) where TUnitOfWork : DbContext
        {
            services.AddConfiguredMsSqlDbContext<TUnitOfWork>(ConfigurationAccessor.GetConnectionString().DefaultString, ConfigurationAccessor.GetUseInMemoryDatabase());

            //var jss = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //    TypeNameHandling = TypeNameHandling.Auto,
            //    Converters = { new SpecialTypesConverter() }
            //};

            //const string redisConfigurationKey = "Redis";
            //services.AddSingleton(typeof(ICacheManagerConfiguration),
            //    new CacheManager.Core.ConfigurationBuilder()
            //        .WithJsonSerializer(serializationSettings: jss, deserializationSettings: jss)
            //        .WithUpdateMode(CacheUpdateMode.Up)
            //        .WithRedisConfiguration(redisConfigurationKey, config =>
            //        {
            //            config.WithAllowAdmin()
            //                .WithDatabase(1)
            //                .WithEndpoint(ConfigurationAccessor.GetRedisConfiguration().Host,
            //                    ConfigurationAccessor.GetRedisConfiguration().Port)
            //                .WithPassword(ConfigurationAccessor.GetRedisConfiguration().Password)
            //                .EnableKeyspaceEvents();
            //        })
            //        .WithMaxRetries(100)
            //        .WithRetryTimeout(100)
            //        .WithRedisCacheHandle(redisConfigurationKey)
            //        .Build());
            //services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));

            //services.AddEFSecondLevelCache(options =>
            //    options.UseCacheManagerCoreProvider()
            //        .DisableLogging(false)
            //        .UseCacheKeyPrefix("EF_")
            //        .CacheQueriesContainingTypes(CacheExpirationMode.Sliding, TimeSpan.FromHours(8), types)
            //);
        }


    }
}