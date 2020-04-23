using Abarnathy.DemographicsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Abarnathy.DemographicsAPI.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the DbContext.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Debug("ConnectionString: {0}", configuration.GetConnectionString("DefaultConnection"));

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

        /// <summary>
        /// Configures Swagger.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DemographicsAPI",
                    Version = "v1",
                    Description = "Abarnathy Patient Demographics API",
                    Contact = new OpenApiContact
                    {
                        Name = "Jon Karlsen",
                        Email = "karlsen.jonarild@gmail.com",
                        Url = new Uri("https://github.com/jakarlse88")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });
        }
    }
}
