using Eefa.Persistence.Data.Redis;
using Eefa.Persistence.Data.SqlServer;
using Library.Configurations;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eefa.Persistence.DataConfiguration
{
    public static  class DataDependencyInjector
    {
        public static void IncludeSqlServer<TDbContext, TUnitOfWork>(this IServiceCollection services) where TDbContext : DbContext where TUnitOfWork : IUnitOfWork
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(TDbContext));
            services.AddDbContext<TDbContext>(options => options.UseSqlServer(DependencyInjector.ConfigurationAccessor.GetConnectionString().DefaultString), ServiceLifetime.Transient);
            services.AddScoped(typeof(TUnitOfWork), typeof(TDbContext));
            services.AddScoped(typeof(IRepository), typeof(Repository));
        }

        public static void IncludeRedis(this IServiceCollection services)
        {
            services.AddScoped<IRedisConfig, RedisConfig>(o =>
                new RedisConfig(DependencyInjector.ConfigurationAccessor.GetRedisConfiguration().Host,
                    DependencyInjector.ConfigurationAccessor.GetRedisConfiguration().Port,
                    DependencyInjector.ConfigurationAccessor.GetRedisConfiguration().Password));

           services.AddScoped<IRedisDataProvider,RedisDataProvider>();
        }

        public static void IncludeRedis<TRedisDataProvider>(this IServiceCollection services) where TRedisDataProvider : IRedisDataProvider
        {
            services.AddScoped<IRedisConfig, RedisConfig>(o =>
                new RedisConfig(DependencyInjector.ConfigurationAccessor.GetRedisConfiguration().Host,
                    DependencyInjector.ConfigurationAccessor.GetRedisConfiguration().Port,
                    DependencyInjector.ConfigurationAccessor.GetRedisConfiguration().Password));

            services.AddScoped(typeof(IRedisDataProvider), typeof(TRedisDataProvider));
        }
    }
}