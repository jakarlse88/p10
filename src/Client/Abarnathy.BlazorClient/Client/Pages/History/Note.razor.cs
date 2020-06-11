using System;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.History
{
    public partial class Note
    {
        [Parameter] public string NoteId { get; set; }
        [Parameter] public int PatientId { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        private NoteInputModel NoteModel { get; set; }
        private APIOperationStatus OperationStatus { get; set; }
        
        /// <summary>
        /// Component initialisation logic.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            NoteModel = null;
            OperationStatus = APIOperationStatus.Initial;
            StateHasChanged();
            
            try
            {
                var response = await HttpClient.GetAsync($"http://localhost:8082/api/history/note/{NoteId}");

                if ((int) response.StatusCode == 200)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<NoteInputModel>(stringContent);

                    NoteModel = content;
                }
                
                OperationStatus = APIOperationStatus.GET_Success;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                OperationStatus = APIOperationStatus.GET_Error;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Return to the relevant Patient details page.
        /// </summary>
        private void NavigateBackToPatient()
        {
            NavigationManager.NavigateTo($"/patient/{PatientId}");
        }

        /// <summary>
        /// Navigate to the Audit Log page.
        /// </summary>
        private void NavigateToAuditLog()
        {
            NavigationManager.NavigateTo($"/Patient/{PatientId}/History/Note/Log/{NoteId}");
        }

        /// <summary>
        /// Navigate to note edit functionality.
        /// </summary>
        private void NavigateToEdit()
        {
            NavigationManager.NavigateTo($"/Patient/{PatientId}/History/Note/Edit/{NoteId}");
        }
    }
}