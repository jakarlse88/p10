using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;

namespace Abarnathy.HistoryAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalService : IExternalService
    {
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

            using var client = new HttpClient();

            await retry.Execute(async () =>
            {
                var response = await client.GetAsync($"http://demographics_api:80/api/Patient/Exists/{id}");

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                patientExists = JsonConvert.DeserializeObject<bool>(responseBody);
            });

            return patientExists;
        }
    }
}