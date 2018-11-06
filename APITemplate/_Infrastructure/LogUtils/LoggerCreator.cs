using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Core;
using System;
using System.IO;

namespace APITemplate._Infrastructure.LogUtils
{
    public class LoggerCreator
    {
        public static Logger CreateLoggerConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var logger = new LoggerConfiguration();

            if (environment == EnvironmentName.Production)
            {
                logger.MinimumLevel.Information();
            }
            else
            {
                logger.MinimumLevel.Debug();
            }

            return logger
                .WriteTo.File(GetPath(environment), rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
        }

        private static string GetPath(string environment)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory + $"logs\\{environment}\\", $"service-.log");
        }
    }
}
