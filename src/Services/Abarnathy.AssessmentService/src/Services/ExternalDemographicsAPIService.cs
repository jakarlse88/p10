using System;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.AssessmentService.Models;
using Abarnathy.AssessmentService.Utilities;
using Polly;
using Serilog;

namespace Abarnathy.AssessmentService.Services
{
    public class ExternalDemographicsAPIService : IExternalDemographicsAPIService
    {
        private readonly HttpClient _httpClient;

        public ExternalDemographicsAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PatientModel> GetPatient(int patientId)
        {
            var retry = Policy.Handle<HttpRequestException>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5)
                });

            try
            {
                PatientModel result = null;
                
                await retry.Execute(async () =>
                {
                    var response =
                        await _httpClient.GetAsync($"/api/patient/{patientId}");

                    response.EnsureSuccessStatusCode();

                    var stream = await response.Content.ReadAsStreamAsync();
                    result = JsonUtilities.DeserializeJsonFromStream<PatientModel>(stream);
                });
                
                return result;
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while attempting to fetch data from an external API.", e.Message);
                throw;
            }
        }
    }
}