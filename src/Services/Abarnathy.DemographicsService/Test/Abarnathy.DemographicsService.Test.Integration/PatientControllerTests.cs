using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Abarnathy.DemographicsService;
using Abarnathy.DemographicsService.Data;
using Abarnathy.DemographicsService.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;

namespace Abarnathy.DemographicsAPI.Test.Integration
{
    public class PatientControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public PatientControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GET_AllPatients()
        {
            //Arrange
            var client = _factory.CreateDefaultClient();

            // Act
            var httpResponse = await client.GetAsync("api/patient");

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var patients = JsonConvert.DeserializeObject<IEnumerable<PatientInputModel>>(stringResponse);
            Assert.NotNull(patients);
            var patientInputModels = patients as PatientInputModel[] ?? patients.ToArray();
            Assert.Equal(2, patientInputModels.Count());
            Assert.Contains(patientInputModels, p => p.GivenName == "Jane");
            Assert.Contains(patientInputModels, p => p.GivenName == "John");
        }

        [Fact]
        public async Task GET_AllPatients_NoRecords()
        {
            //Arrange
            var client = CreateClientNoDbSeed();

            // Act
            var httpResponse = await client.GetAsync("api/patient");

            // Assert
            Assert.Equal(404, (int) httpResponse.StatusCode);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal("No entities found.", stringResponse);
        }


        [Fact]
        public async Task GET_SinglePatient_ValidId()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();

            // Act
            var httpResponse = await client.GetAsync("api/patient/1");

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var patient = JsonConvert.DeserializeObject<PatientInputModel>(stringResponse);
            Assert.NotNull(patient);
            Assert.Equal("Jane", patient.GivenName);
        }

        [Fact]
        public async Task GET_SinglePatient_IdInvalid()
        {
            // Arrange
            var client = CreateClientNoDbSeed();

            // Act
            var httpResponse = await client.GetAsync("api/patient/3");

            // Assert
            Assert.Equal(204, (int) httpResponse.StatusCode);
        }

        [Fact]
        public async Task GET_SinglePatient_IdBad()
        {
            // Arrange
            var client = CreateClientNoDbSeed();

            // Act
            var httpResponse = await client.GetAsync("api/patient/0");

            // Assert
            Assert.Equal(400, (int) httpResponse.StatusCode);
        }

        [Fact]
        public async Task POST_ModelNull_ReturnsBadRequest()
        {
            // Arrange
            var client = CreateClientNoDbSeed();

            // Act
            var json = JsonConvert.SerializeObject(null);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync("api/patient", stringContent);

            // Assert
            Assert.Equal(400, (int) httpResponse.StatusCode);
        }

        [Fact]
        public async Task POST_ModelValid_ReturnsCreatedAtAction()
        {
            // Arrange
            var client = CreateClientNoDbSeed();

            // Act
            var testModel = new PatientInputModel
            {
                GivenName = "Jon",
                FamilyName = "Karlsen",
                DateOfBirth = new DateTime(1988 - 07 - 04),
                SexId = 1,
                Addresses = new List<AddressInputModel>
                {
                    new AddressInputModel
                    {
                        StreetName = "Baker St.",
                        HouseNumber = "6",
                        Town = "Baskerville",
                        State = "Washington",
                        ZipCode = "12345"
                    },
                    new AddressInputModel
                    {
                        StreetName = "Baker St.",
                        HouseNumber = "7",
                        Town = "Baskerville",
                        State = "Washington",
                        ZipCode = "12345"
                    },
                },
                PhoneNumbers = new List<PhoneNumberInputModel>
                {
                    new PhoneNumberInputModel { Number = "1234567890" },
                    new PhoneNumberInputModel { Number = "0987654321" },
                }
            };

            var json = JsonConvert.SerializeObject(testModel);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync("api/patient", stringContent);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal(201, (int) httpResponse.StatusCode);
        }

        [Fact]
        public async Task PUT_IdBad_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();
            var json = JsonConvert.SerializeObject(new PatientInputModel
            {
                GivenName = "Jonnn",
                FamilyName = "Karlsennn",
                DateOfBirth = new DateTime(1988 - 07 - 04),
                SexId = 1,
                Addresses = new List<AddressInputModel>
                {
                    new AddressInputModel
                    {
                        StreetName = "Baker St.",
                        HouseNumber = "6",
                        Town = "Baskerville",
                        State = "Washington",
                        ZipCode = "12345"
                    }
                },
                PhoneNumbers = new List<PhoneNumberInputModel>
                {
                    new PhoneNumberInputModel { Number = "0987654321" }
                }
            });
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await client.PutAsync("api/patient/0", stringContent);

            // Assert
            Assert.Equal(400, (int) httpResponse.StatusCode);
        }

        [Fact]
        public async Task PUT_ModelNull_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();
            var json = JsonConvert.SerializeObject(null);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await client.PutAsync("api/patient/1", stringContent);

            // Assert
            Assert.Equal(400, (int) httpResponse.StatusCode);
        }

        [Fact]
        public async Task PUT_EntityNotFound_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();
            var json = JsonConvert.SerializeObject(new PatientInputModel
            {
                GivenName = "Jonnn",
                FamilyName = "Karlsennn",
                DateOfBirth = new DateTime(1988 - 07 - 04),
                SexId = 1,
                Addresses = new List<AddressInputModel>
                {
                    new AddressInputModel
                    {
                        StreetName = "Baker St.",
                        HouseNumber = "6",
                        Town = "Baskerville",
                        State = "Washington",
                        ZipCode = "12345"
                    }
                },
                PhoneNumbers = new List<PhoneNumberInputModel>
                {
                    new PhoneNumberInputModel { Number = "0987654321" }
                }
            });
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await client.PutAsync("api/patient/10", stringContent);

            // Assert
            Assert.Equal(404, (int) httpResponse.StatusCode);
        }

        [Fact]
        public async Task PUT_EntityFound_ReturnsNoContent()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();
            var json = JsonConvert.SerializeObject(new PatientInputModel
            {
                GivenName = "Jonnn",
                FamilyName = "Karlsennn",
                DateOfBirth = new DateTime(1988 - 07 - 04),
                SexId = 1,
                Addresses = new List<AddressInputModel>(),
                PhoneNumbers = new List<PhoneNumberInputModel>()
            });
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await client.PutAsync("api/patient/1", stringContent);

            // Assert
            Assert.Equal(204, (int) httpResponse.StatusCode);
        }

        [Fact]
        public async Task PUT_IncomingAddressesAndPhoneNumbers_ReturnsNoContent()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();
            var json = JsonConvert.SerializeObject(new PatientInputModel
            {
                GivenName = "Jonnn",
                FamilyName = "Karlsennn",
                DateOfBirth = new DateTime(1988 - 07 - 04),
                SexId = 1,
                Addresses = new List<AddressInputModel>
                {
                    new AddressInputModel
                    {
                        StreetName = "Baker St.",
                        HouseNumber = "6",
                        Town = "Baskerville",
                        State = "Washington",
                        ZipCode = "12345"
                    },
                    new AddressInputModel
                    {
                        StreetName = "Baker St.",
                        HouseNumber = "7",
                        Town = "Baskerville",
                        State = "Washington",
                        ZipCode = "12345"
                    },
                    new AddressInputModel
                    {
                        StreetName = "Baker St.",
                        HouseNumber = "8",
                        Town = "Baskerville",
                        State = "Washington",
                        ZipCode = "12345"
                    }
                },
                PhoneNumbers = new List<PhoneNumberInputModel>
                {
                    new PhoneNumberInputModel
                    {
                        Number = "1234567890",
                    },
                    new PhoneNumberInputModel
                    {
                        Number = "1234567890",
                    },
                    new PhoneNumberInputModel
                    {
                        Number = "0987654321",
                    }
                }
            });
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await client.PutAsync("api/patient/1", stringContent);

            // Assert
            Assert.Equal(204, (int) httpResponse.StatusCode);
        }


        [Fact]
        public async Task GET_PatientExists_BadId_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();

            // Act
            var httpResponse = await client.GetAsync("api/Patient/Exists/0");

            // Assert
            Assert.Equal(400, (int) httpResponse.StatusCode);
        }

        [Fact]
        public async Task GET_PatientExists_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();

            // Act
            var httpResponse = await client.GetAsync("api/Patient/Exists/3");

            // Assert
            Assert.Equal(404, (int) httpResponse.StatusCode);
        }

        [Fact]
        public async Task GET_PatientExists_ValidId_ReturnsNoContent()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();

            // Act
            var httpResponse = await client.GetAsync("api/Patient/Exists/1");

            // Assert
            Assert.Equal(204, (int) httpResponse.StatusCode);
        }

        
        /**
         * ==========================================================
         * Private helper methods
         * ==========================================================
         */
        private HttpClient CreateClientNoDbSeed()
        {
            return _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove the app's ApplicationDbContext registration.
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                 typeof(DbContextOptions<DemographicsDbContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        // Add ApplicationDbContext using an in-memory database for testing.
                        services.AddDbContext<DemographicsDbContext>(options =>
                        {
                            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
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
                                .GetRequiredService<ILogger<CustomWebApplicationFactory<Startup>>>();

                            // Ensure the database is created.
                            db.Database.EnsureCreated();
                            // try
                            // {
                            //     // Seed the database with test data.
                            //     Utilities.InitializeDbForTests(db);
                            // }
                            // catch (Exception ex)
                            // {
                            //     logger.LogError(ex, "An error occurred seeding the " +
                            //                         "database with test messages. Error: {Message}", ex.Message);
                            // }
                        }
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }
    }
}