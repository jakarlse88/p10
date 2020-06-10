using System;
using System.Collections.Generic;
using System.Net;
using Abarnathy.HistoryService.Data;
using Abarnathy.HistoryService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Polly;
using Serilog;

namespace Abarnathy.HistoryService.Infrastructure
{
    /// <summary>
    /// IApplicationBuilder extension methods.
    /// </summary>
    internal static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Applies pending migrations.
        /// </summary>
        /// <param name="app"></param>
        internal static void ApplyMigrations(this IApplicationBuilder app)
        {
            Log.Information("Seeding database...");

            using (var serviceScope = app.ApplicationServices.CreateScope())
            using (var context = serviceScope.ServiceProvider.GetRequiredService<PatientHistoryDbContext>())
            {
                try
                {
                    var retry = Policy.Handle<MongoException>()
                        .WaitAndRetry(new[]
                        {
                            TimeSpan.FromSeconds(120),
                            TimeSpan.FromSeconds(90),
                            TimeSpan.FromSeconds(60)
                        });

                    retry.Execute(() => { context.Notes.InsertMany(GenerateSeedData()); });
                }
                catch (Exception e)
                {
                    Log.Error("There was an error applying migrations. Exception: {e}", e);
                    throw;
                }
            }

            Log.Information("Seeding successfully completed.");
        }


        /// <summary>
        /// Configure Swagger UI.
        /// </summary>
        /// <param name="app"></param>
        internal static void UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abarnathy Patient History API 1.0");
                c.RoutePrefix = string.Empty;
            });
        }

        /// <summary>
        /// Configures the global exception handler middleware.
        /// </summary>
        /// <param name="app"></param>
        internal static void UseCustomExceptionHandlerExtension(this IApplicationBuilder app)
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

        private static IEnumerable<Note> GenerateSeedData()
        {
            return new[]
            {
                new Note
                {
                    PatientId = 1,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n" +
                              "Abnormal\n" +
                              "Cholesterol\n" +
                              "Dizziness\n" +
                              "Relapse\n" +
                              "Reaction\n" +
                              "Antibodies\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                },
                new Note
                {
                    PatientId = 2,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n" +
                              "Abnormal\n" +
                              "Cholesterol\n" +
                              "Dizziness\n" +
                              "Relapse\n" +
                              "Reaction\n" +
                              "Antibodies\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                },
                new Note
                {
                    PatientId = 3,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n" +
                              "Abnormal\n" +
                              "Cholesterol\n" +
                              "Dizziness\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                },
                new Note
                {
                    PatientId = 4,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n" +
                              "Abnormal\n" +
                              "Cholesterol\n" +
                              "Dizziness\n" +
                              "Relapse\n" +
                              "Reaction\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                },
                new Note
                {
                    PatientId = 5,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n" +
                              "Abnormal\n" +
                              "Cholesterol\n" +
                              "Dizziness\n" +
                              "Relapse\n" +
                              "Reaction\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                },
                new Note
                {
                    PatientId = 6,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n" +
                              "Abnormal\n" +
                              "Cholesterol\n" +
                              "Dizziness\n" +
                              "Relapse\n" +
                              "Reaction\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                },
                new Note
                {
                    PatientId = 7,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n" +
                              "Abnormal\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                },
                new Note
                {
                    PatientId = 8,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n" +
                              "Abnormal\n" +
                              "Reaction\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                },
                new Note
                {
                    PatientId = 9,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                },
                new Note
                {
                    PatientId = 10,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                },
                new Note
                {
                    PatientId = 11,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                },
                new Note
                {
                    PatientId = 12,
                    Title = "Test",
                    Content = "Weight\n" +
                              "Smoker\n",
                    TimeCreated = DateTime.Now,
                    TimeLastUpdated = DateTime.Now
                }
            };
        }
    }
}