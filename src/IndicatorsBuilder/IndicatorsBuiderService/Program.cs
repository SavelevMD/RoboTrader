using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using IndicatorsBuilderService.Logic.Extensions;

using NLog;
using NLog.Web;
using Microsoft.Extensions.Logging;

namespace IndicatorsBuiderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddConsole();
                    builder.AddNLog("nlog.config");
                });
    }
}
