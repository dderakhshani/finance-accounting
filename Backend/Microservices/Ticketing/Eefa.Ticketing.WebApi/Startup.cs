using Eefa.Common.Web;
using Eefa.Ticketing.Application;
using Eefa.Ticketing.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ticketing
{
    public class Startup
    {
        //private readonly string _mainCrossOriginName = "MainCrossOrigin";
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: _mainCrossOriginName,
            //                      builder =>
            //                      {
                                     
            //                              builder.AllowAnyOrigin();
                                      
            //                          builder.AllowAnyHeader();
            //                          builder.AllowAnyMethod();
            //                      });
            //});

            services.IncludeBaseServices(Configuration);
            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ticketing", Version = "v1" });
            //});

            services.AddInfrastructureSetup(Configuration);
            services.AddApplicationServicesSetup();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticketing v1"));
            //}

            StartupConfig.Initialize(new ConfigurationAccessor(Configuration));
            app.IncludeAll();
            //app.UseCors(_mainCrossOriginName);
            //app.UseHttpsRedirection();

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
