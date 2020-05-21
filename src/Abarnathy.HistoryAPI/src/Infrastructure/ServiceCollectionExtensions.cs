using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Abarnathy.HistoryAPI.Data;
using Abarnathy.HistoryAPI.Repositories;
using Abarnathy.HistoryAPI.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Abarnathy.HistoryAPI.Infrastructure
{
    /// <summary>
    /// IServiceCollection extension methods (for configuration).
    /// </summary>
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the app's local services.
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureLocalServices(this IServiceCollection services)
        {
            services.AddTransient<INoteService, NoteService>();
            services.AddTransient<IExternalService, ExternalService>();
            
            services.AddScoped<INoteRepository, NoteRepository>();
        }

        /// <summary>
        /// Configures controllers with action filters.
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                var noContentFormatter =
                    options.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();

                if (noContentFormatter != null)
                {
                    noContentFormatter.TreatNullValueAsNoContent = false;
                }
            });
        }

        /// <summary>
        /// Configures the global CORS policy.
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder // For testing using docker-compose
                        .WithOrigins("http://localhost:8081")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();

                    builder // For testing with non-container client
                        .WithOrigins("http://localhost:5000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        /// <summary>
        /// Configures the DbContext.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        internal static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            ConventionRegistry.Register("CamelCase", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);

            services.AddSingleton<IMongoClient>(s =>
                new MongoClient(configuration["PatientHistoryDatabaseSettings:ConnectionString"]));

            services.AddScoped(s => new PatientHistoryDbContext(s.GetRequiredService<IMongoClient>(),
                configuration["PatientHistoryDatabaseSettings:DatabaseName"]));
        }

        /// <summary>
        /// Configures Swagger.
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HistoryAPI",
                    Version = "v1",
                    Description = "Abarnathy Patient History API",
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
