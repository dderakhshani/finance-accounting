using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace FileTransfer.WebApi
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
                    webBuilder.UseStartup<Startup>().ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.AddDebug();
                            logging.AddConsole();
                            logging.SetMinimumLevel(LogLevel.Debug);
                        })
                        .UseNLog();
                });
    }
}