//using Eefa.Accounting.Persistence.Repositories;
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
        //services.AddScoped<IGenericRepository<AccountReference>, GenericRepository<AccountReference>>();
        //services.AddScoped<IGenericRepository<AccountReferencesGroup>, GenericRepository<AccountReferencesGroup>>();
        //services.AddScoped<IGenericRepository<AutoVoucherFormula>, GenericRepository<AutoVoucherFormula>>();
        //services.AddScoped<IGenericRepository<CodeRowDescription>, GenericRepository<CodeRowDescription>>();
        //services.AddScoped<IGenericRepository<CodeVoucherExtendType>, GenericRepository<CodeVoucherExtendType>>();
        //services.AddScoped<IGenericRepository<CodeVoucherGroup>, GenericRepository<CodeVoucherGroup>>();
        //services.AddScoped<IGenericRepository<MoadianInvoiceHeader>, GenericRepository<MoadianInvoiceHeader>>();
        //services.AddScoped<IGenericRepository<VouchersHead>, GenericRepository<VouchersHead>>();



        return services;
    }
}
