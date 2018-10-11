using System;
using System.IO;
using APITemplate._Infrastructure.LogUtils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace APITemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = LoggerCreator.CreateLoggerConfiguration();

            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
           .ConfigureAppConfiguration(ConfigConfiguration)
           .UseStartup<Startup>()
               .UseSerilog()
               .Build();

        static void ConfigConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder config)
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        }
    }
}
