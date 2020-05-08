using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Abarnathy.DemographicsAPI.Test.Integration
{
    public class PatientControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public PatientControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task CanGetPatients()
        {
            // Arrange
            
            // Act
            var httpResponse = await _client.GetAsync("api/patient");

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var patients = JsonConvert.DeserializeObject<IEnumerable<PatientInputModel>>(stringResponse);
            
            Assert.Equal(2, patients.Count());
            Assert.Contains(patients, p => p.GivenName == "Jane");
            Assert.Contains(patients, p => p.GivenName == "John");
        }

    }
}