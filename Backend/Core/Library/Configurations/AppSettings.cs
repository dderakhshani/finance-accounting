using System.Globalization;
using System.Linq;
using Library.Exceptions.Middleware;
using Library.Interfaces;
using Library.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Library.Configurations
{
    public static class AppSettings
    {
        private static IConfigurationAccessor _configurationAccessor;

        public static void LoadConfigurations(IConfigurationAccessor configurationAccessor)
        {
            _configurationAccessor = configurationAccessor;
        }


        public static void IncludeCorsPolicy(this IApplicationBuilder app)
        {
            app.UseCors(_configurationAccessor.GetCorsConfiguration().PolicyName);
        }

        public static void IncludeBuffering(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });
        }


        //public static void IncludeAuditMiddleware(this IApplicationBuilder app)
        //{
        //    //app.UseAuditMiddleware();
        //    app.UseAuditCustomAction(new CurrentUserAccessor.CurrentUserAccessor(new HttpContextAccessor()));
        //}


        public static void IncludeExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }

        public static void IncludeRouting(this IApplicationBuilder app)
        {
            app.UseRouting();
        }
        public static void IncludeRequestLocalization(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(app.ApplicationServices
                .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        }

        public static void IncludeSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/{_configurationAccessor.GetSwaggerConfiguration().Name}/swagger.json", $"{_configurationAccessor.GetSwaggerConfiguration().Title} {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}"); });
        }

        public static void IncludeAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
        }

        public static void IncludeAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthorization();
        }

        public static void IncludeRequestResourcesLocalization(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(app.ApplicationServices
                .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        }
        public static void IncludeEndpoint(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            var requestLocalizationOptions = new RequestLocalizationOptions();

            requestLocalizationOptions.SupportedCultures = requestLocalizationOptions.SupportedUICultures =
                new CultureInfo[] { new CultureInfo("en"), new CultureInfo("fa") }.ToList();

            requestLocalizationOptions.RequestCultureProviders.Insert(0, new RouteValueRequestCultureProvider() { Options = requestLocalizationOptions });
            app.UseRequestLocalization(requestLocalizationOptions);

        }

        //this extention method configes the audit middle ware
        //public static void UseAuditMiddleware(this IApplicationBuilder app)
        //{
        //    app.UseAuditMiddleware(_ => _
        //        .IncludeHeaders() // to save requests header
        //        .IncludeResponseBody() // to save response body
        //        .WithEventType("HTTP:{verb}:{url}"));
        //}
    }
}