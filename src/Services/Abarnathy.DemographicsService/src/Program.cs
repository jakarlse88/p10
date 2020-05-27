using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using Microsoft.Data.SqlClient;
using Polly;
using Serilog.Sinks.SystemConsole.Themes;

namespace Abarnathy.DemographicsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            try
            {
                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(new []
                    {
                        TimeSpan.FromSeconds(120),
                        TimeSpan.FromSeconds(90),
                        TimeSpan.FromSeconds(60)
                    });

                retry.Execute(() =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console(
                            outputTemplate:
                            "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                            theme: AnsiConsoleTheme.Literate)
                        .WriteTo.MSSqlServer(
                            connectionString: appSettings.GetConnectionString("DefaultConnection"),
                            tableName: "Log",
                            autoCreateSqlTable: true)
                        .Enrich.FromLogContext()
                        .CreateLogger();
                });
            }
            catch (Exception e)
            {
                Log.Error("There was an error configuring Serilog. Exception: {e}", e);
                throw;
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}