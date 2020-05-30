using System;
using System.IO;
using System.Reflection;
using Abarnathy.AssessmentService.Models;
using Abarnathy.AssessmentService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Abarnathy.AssessmentService.Infrastructure
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the global CORS policy.
        /// </summary>
        /// <param name="services"></param>
        internal static IServiceCollection ConfigureCors(this IServiceCollection services)
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

            return services;
        }

        internal static IServiceCollection ConfigureLocalServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddTransient<IAssessmentService, Services.RiskAssessmentService>()
                .AddExternalDemographicsAPIService<ExternalDemographicsAPIService>(
                    configuration["DEMOGRAPHICS_SERVICE_BASE_ADDRESS"])
                .AddExternalHistoryAPIService<ExternalHistoryAPIService>(
                    configuration["HISTORY_SERVICE_BASE_ADDRESS"]);

            return services;
        }
        
        private static IServiceCollection AddExternalDemographicsAPIService<TImplementation>(
            this IServiceCollection services, string baseAddress)
            where TImplementation : class, IExternalDemographicsAPIService
        {
            services.AddHttpClient<IExternalDemographicsAPIService, TImplementation>(cfg =>
            {
                cfg.BaseAddress = new Uri(baseAddress);
            });

            return services;
        }

        private static IServiceCollection AddExternalHistoryAPIService<TImplementation>(
            this IServiceCollection services, string baseAddress)
            where TImplementation : class, IExternalHistoryAPIService
        {
            services.AddHttpClient<IExternalHistoryAPIService, TImplementation>(cfg =>
            {
                cfg.BaseAddress = new Uri(baseAddress);
            });

            return services;
        }
        
        
        /// <summary>
        /// Configures Swagger.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Assessment Service",
                    Version = "v1",
                    Description = "Abarnathy Patient Assessment Service API",
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

            return services;
        }
    }
}