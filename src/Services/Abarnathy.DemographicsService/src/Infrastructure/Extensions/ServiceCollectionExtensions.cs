using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Abarnathy.DemographicsService.Data;
using Abarnathy.DemographicsService.Services;
using Abarnathy.DemographicsService.Services.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;

namespace Abarnathy.DemographicsService.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the app's local services.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureLocalServices(this IServiceCollection services)
        {
            services.AddTransient<IPatientService, PatientService>();
        }

        /// <summary>
        /// Configures controllers with action filters,
        /// and configures and adds FluentValidation.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    var noContentFormatter =
                        options.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();

                    if (noContentFormatter != null)
                    {
                        noContentFormatter.TreatNullValueAsNoContent = false;
                    }
                })
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                    fv.ImplicitlyValidateChildProperties = true;
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        /// <summary>
        /// Configures the global CORS policy.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
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
        /// <param name="environment"></param>
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddDbContext<DemographicsDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions
                            .MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);

                        sqlOptions
                            .EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
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
                    Title = "Demographics Service",
                    Version = "v1",
                    Description = "Abarnathy Patient Demographics Service API",
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