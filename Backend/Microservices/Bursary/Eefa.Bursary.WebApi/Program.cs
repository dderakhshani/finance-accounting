using System;
using Eefa.Bursary.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using SharedCode;

namespace Eefa.Bursary.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SharedProgram.SetupRunOld<BursaryContext,Startup>(args);
        }
    }
}
