using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureAppConfiguration((context, config) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                        config.AddJsonFile($"ocelot-config.{env}.json");
                    else
                        config.AddJsonFile($"ocelot-config.json");
                });

            }).ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddDebug();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);
            }).UseNLog();
    }
}
