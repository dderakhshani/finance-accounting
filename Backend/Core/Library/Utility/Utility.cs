using System;
using System.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Library.Utility
{
    public static class Utility
    {
        public static string ConvertNumberToEnglish(string number)
        {
            number = number.Trim();
            if (string.IsNullOrEmpty(number)) return number;
            number = number.Replace('٠', '0');
            number = number.Replace('١', '1');
            number = number.Replace('٢', '2');
            number = number.Replace('٣', '3');
            number = number.Replace('٤', '4');
            number = number.Replace('٥', '5');
            number = number.Replace('٦', '6');
            number = number.Replace('٧', '7');
            number = number.Replace('٨', '8');
            number = number.Replace('٩', '9');
            return number;
        }

        public static T EvaluatString<T>(string expression)
        {
            var dt = new DataTable();
            return (T)dt.Compute(expression, "");
        }
        public static string EvaluatString(string expression)
        {
            var dt = new DataTable();
            return dt.Compute(expression, "").ToString() ?? throw new InvalidOperationException();
        }

        public static IWebHost BuildWebHost<T>() where T : class
        {
            return WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();
                    config.AddConfiguration(hostingContext.Configuration);
                    config.AddJsonFile("appsettings.json");
                    
                })
                .ConfigureServices(sc =>
                {
                })
                .UseStartup<T>()
                .Build();
        }

        public static WebApplicationFactory<T> BuildWebApplicationFactory<T>() where T : class
        {
            return new WebApplicationFactory<T>();
        }
    }
}