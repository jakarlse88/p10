using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.AssessmentService.Models;
using Abarnathy.AssessmentService.Utilities;
using Polly;
using Serilog;

namespace Abarnathy.AssessmentService.Services
{
    public class ExternalHistoryAPIService : IExternalHistoryAPIService
    {
        private readonly HttpClient _httpClient;
        
        public ExternalHistoryAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<IEnumerable<NoteModel>> GetNotes(int patientId)
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
                await retry.Execute(async () =>
                {
                    var response =
                        await _httpClient.GetAsync($"/api/history/patient/{patientId}");

                    response.EnsureSuccessStatusCode();

                    var stream = await response.Content.ReadAsStreamAsync();
                    return JsonUtilities.DeserializeJsonFromStream<IEnumerable<NoteModel>>(stream);
                });
                
                return new List<NoteModel>();
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while attempting to fetch data from an external API.", e.Message);
                throw;
            }
        }
    }
}