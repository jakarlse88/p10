using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Models;
using Newtonsoft.Json;
using Xunit;

namespace Abarnathy.DemographicsAPI.Test.Integration
{
    public class PatientControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;

        public PatientControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task CanGetPatients()
        {
            // Arrange

            // Act
            var httpResponse = await _httpClient.GetAsync("api/patient");

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var patients = JsonConvert.DeserializeObject<IEnumerable<PatientDTO>>(stringResponse);
            
            Assert.Equal(2, patients.Count());
            Assert.Contains(patients, p => p.GivenName == "Jane");
            Assert.Contains(patients, p => p.GivenName == "John");
        }

    }
}