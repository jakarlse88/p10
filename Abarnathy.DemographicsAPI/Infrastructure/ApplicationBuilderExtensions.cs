using Abarnathy.DemographicsAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Serilog;
using System;

namespace Abarnathy.DemographicsAPI.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            using (var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                try
                {
                    var retry = Policy.Handle<SqlException>()
                        .WaitAndRetry(new TimeSpan[]
                        {
                            TimeSpan.FromSeconds(120),
                            TimeSpan.FromSeconds(90),
                            TimeSpan.FromSeconds(60)
                        });

                    retry.Execute(() => context.Database.Migrate());
                }
                catch (Exception e)
                {
                    Log.Error("There was an error applying migrations. Exception: {e}", e);
                    throw;
                }
            }
        }
    }
}
