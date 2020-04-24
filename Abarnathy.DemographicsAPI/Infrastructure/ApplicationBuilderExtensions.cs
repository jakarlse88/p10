﻿using Abarnathy.DemographicsAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Serilog;
using System;
using Microsoft.AspNetCore.HttpOverrides;

namespace Abarnathy.DemographicsAPI.Infrastructure
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
                    Log.Information("Database is not up-to-date.");
                    Log.Information("Applying migrations. This may take some time.");

                    try
                    {
                        var retry = Policy.Handle<SqlException>()
                            .WaitAndRetry(new TimeSpan[]
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abarnathy Demographics API 1.0");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}