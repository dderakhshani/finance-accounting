using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using NLog;
namespace Eefa.Commodity.WebApi
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            
            logger.Trace("Some verbose log");
            logger.Debug("Some debug log");


            logger.Error("Error accrued at {now}", DateTime.Now);

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


    }
}
