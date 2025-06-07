using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

     //   services.AddSignalR();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        //services.AddSingleton<IVoucherHeadCacheServices,VoucherHeadCacheServices>();
        return services;
    }
}
