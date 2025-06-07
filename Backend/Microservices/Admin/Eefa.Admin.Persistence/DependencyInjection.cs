using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultString")));


        services.AddScoped<IUnitOfWork, UnitOfWork>();


        //services.AddScoped<IGenericRepository<AccountHead>, AccountHeadRepository>();
        return services;
    }
}
