    using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Eefa.Common.Common.Exceptions;
using Eefa.Common.Exceptions;
using Eefa.NotificationServices.Services.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Eefa.Common.Web
{
    public static class StartupConfig
    {
        private static IConfigurationAccessor _configurationAccessor;

        public static void Initialize(IConfigurationAccessor configurationAccessor)
        {
            _configurationAccessor = configurationAccessor;
        }


        public static void IncludeBase(this IApplicationBuilder app)
        {
            app.IncludeBuffering();
            app.UseExceptionHandler(ExceptionHandler);
            //app.UseAuditMiddleware();
            //app.UseAuditCustomAction(new CurrentUserAccessor(new HttpContextAccessor()));
            app.IncludeCorsPolicy();
            app.IncludeSwagger();
            app.IncludeRouting();
            app.IncludeAuthentication();
            app.IncludeAuthorization();
            app.IncludeEndpoint();
        }

        public static void IncludeAll(this IApplicationBuilder app)
        {
            IncludeBase(app);
        }

        private static void IncludeCorsPolicy(this IApplicationBuilder app)
        {            
            app.UseCors(_configurationAccessor.GetCorsConfiguration().PolicyName);
        }

        private static void IncludeBuffering(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });
        }


        private static void IncludeAuditMiddleware(this IApplicationBuilder app)
        {
            //app.UseAuditMiddleware();
            //app.UseAuditCustomAction(new CurrentUserAccessor.CurrentUserAccessor(new HttpContextAccessor()));
        }

        private static void IncludeRouting(this IApplicationBuilder app)
        {
            app.UseRouting();
        }
        private static void IncludeRequestLocalization(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(app.ApplicationServices
                .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        }

        private static void IncludeSwagger(this IApplicationBuilder app)
        {
           
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/{_configurationAccessor.GetSwaggerConfiguration().Name}/swagger.json", $"{_configurationAccessor.GetSwaggerConfiguration().Title} {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}"); });
           
        }

        private static void IncludeAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
        }

        private static void IncludeAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthorization();
        }

        private static void IncludeRequestResourcesLocalization(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(app.ApplicationServices
                .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        }
        private static void IncludeEndpoint(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers(); });         
                        
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
        //        .IncludeRequestBody() // to save requests body
        //        .IncludeResponseBody() // to save response body
        //        .WithEventType("HTTP:{verb}:{url}"));
        //}

        private static void ExceptionHandler(IApplicationBuilder exceptionHandlerApp)
        {
            exceptionHandlerApp.Run(async context =>
            {
                var response = context.Response;
                response.ContentType = "application/json";
                IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

                var error =
              context.Features.Get<IExceptionHandlerFeature>();

                if (error.Error is ValidationException validationException)
                {
                    errors = validationException.Failures;
                    response.StatusCode = 422;//Unprocessable Entity
                }
                if (error.Error is ValidationError Exception)
                {
                    errors.Add("handled exception", new List<string> { error.Error.Message });
                    response.StatusCode = 422;//Unprocessable Entity
                }
                else if (error.Error is IUnAuthorizedException)
                {
                    errors.Add("handled exception", new List<string> { error.Error.Message });
                    response.StatusCode = 401;
                }
                else if (error.Error is IValidationException)
                {
                    errors.Add("handled exception", new List<string> { error.Error.Message });
                    response.StatusCode = 400;
                }
                else if (error.Error is ApplicationValidationException validationsException)
                {
                    errors.Add("handled exception", validationsException.Errors.Select(x => x.Message).ToList());
                    response.StatusCode = 400;
                }
                else
                {
                    errors.Add("unhandled exception", new List<string> { error.Error.Message });
                    response.StatusCode = 500;
                }

                var result = ServiceResult.Failed(errors);
                result.ObjResult = error;
                await response.WriteAsync( JsonConvert.SerializeObject(result));
            });
        }
    }
}