using System;
using System.Linq;
using Abarnathy.HistoryAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Abarnathy.HistoryAPI.Test.Integration
{
    public class CustomWebApplicationFactory<TStartup> 
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var clientDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(IMongoClient));

                if (clientDescriptor != null)
                {
                    services.Remove(clientDescriptor);
                }

                var contextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(PatientHistoryDbContext));

                if (contextDescriptor != null)
                {
                    services.Remove(contextDescriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing.
                services.AddDbContext<DemographicsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<DemographicsDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data.
                        Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}