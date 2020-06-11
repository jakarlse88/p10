using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Serilog;

namespace Abarnathy.HistoryService.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalAPIService : IExternalAPIService
    {
        private readonly HttpClient _httpClient;
        
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="httpClient"></param>
        public ExternalAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Call the DemographicsAPI to ensure that the Patient entity exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> PatientExists(int id)
        {
            var patientExists = false;

            var retry = Policy.Handle<HttpRequestException>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5)
                });

            try
            {

                await retry.Execute(async () =>
                {
                    var response = await _httpClient.GetAsync($"/api/Patient/Exists/{id}");

                    patientExists = response.IsSuccessStatusCode;
                });

                return patientExists;
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while attempting to fetch data from an external API.", e.Message);
                throw;
            }
        }
    }
}