using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate._Infrastructure
{
    public class LoggerHelper
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
