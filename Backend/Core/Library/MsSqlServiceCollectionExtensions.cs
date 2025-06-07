using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library
{
    public static class MsSqlServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguredMsSqlDbContext<TUnitOfWork>(this IServiceCollection services, string connectionString, bool useInMemoryDatabase) where TUnitOfWork:DbContext
        {

            if (useInMemoryDatabase)
            {
                services.AddDbContextPool<TUnitOfWork>(options =>
                    options.UseInMemoryDatabase("EefaTest"));
            }
            else
            {
                services.AddDbContextPool<TUnitOfWork>((serviceProvider, optionsBuilder) =>
                optionsBuilder
                    .UseSqlServer(
                        connectionString,
                        sqlServerOptionsBuilder =>
                        {
                            sqlServerOptionsBuilder
                                .CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)
                                .EnableRetryOnFailure()
                                .MigrationsAssembly(typeof(MsSqlServiceCollectionExtensions).Assembly.FullName)
                                .UseNetTopologySuite();
                        }));
            }
            return services;
        }
    }
}