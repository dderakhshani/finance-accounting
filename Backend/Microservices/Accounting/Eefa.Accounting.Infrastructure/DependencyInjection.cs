using Eefa.NotificationServices.SMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddJwtAuthentication(configuration);

        services.AddScoped<ISMSService, SMSService>();

        return services;
    }
}

