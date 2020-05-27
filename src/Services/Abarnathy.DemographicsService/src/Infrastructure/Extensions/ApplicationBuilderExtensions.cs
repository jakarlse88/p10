using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Serilog;
using System;
using System.Net;
using Abarnathy.DemographicsService.Data;
using Abarnathy.DemographicsService.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;

namespace Abarnathy.DemographicsService.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Applies initial schema migration, if necessary.
        /// </summary>
        /// <param name="app"></param>
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            using (var context = serviceScope.ServiceProvider.GetRequiredService<DemographicsDbContext>())
            {
                if (context.Database.GetService<IRelationalDatabaseCreator>().Exists())
                {
                    Log.Information("Database is up-to-date.");
                }
                else
                {
                    Log.Information("Database is not up-to-date; applying migrations. This may take some time.");

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
                            context.Database.Migrate();
                        });
                    }
                    catch (Exception e)
                    {
                        Log.Error("There was an error applying migrations. Exception: {e}", e);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Configure forwarded headers.
        /// </summary>
        /// <param name="app"></param>
        public static void UseForwardedHeaders(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            });
        }

        /// <summary>
        /// Configure Swagger UI.
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abarnathy Patient Demographics Service API 1.0");
                c.RoutePrefix = string.Empty;
            });
        }

        /// <summary>
        /// Configures the global exception handler middleware.
        /// </summary>
        /// <param name="app"></param>
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                        if (contextFeature != null)
                        {
                            Log.Error("Error: {0}", contextFeature.Error);

                            await context.Response.WriteAsync(new ErrorDetails()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = "Internal Server Error"
                            }.ToString());
                        }
                    });
            });
        }
    }
}