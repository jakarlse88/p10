using Abarnathy.DemographicsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Reflection;

namespace Abarnathy.DemographicsAPI.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Information("ConnectionString: {0}", configuration.GetConnectionString("DefaultConnection"));

            services.AddDbContext<DemographicsDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions
                        .MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);

                    sqlOptions
                        .EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
            });
        }
    }
}
