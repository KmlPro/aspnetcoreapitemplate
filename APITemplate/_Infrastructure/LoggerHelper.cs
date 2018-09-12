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
        public static string _GetPath()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + $"logs\\{environment}\\", $"service-.log");

            return path;
        }

        public static Logger CreateLoggerConfiguration()
        {
#if DEBUG
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(_GetPath(), rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
#else
            return new LoggerConfiguration()
             .MinimumLevel.Information()
             .WriteTo.File(_GetPath(), rollingInterval: RollingInterval.Day, shared: true)
             .CreateLogger();
#endif
        }
    }
}
