using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.History
{
    public partial class CreateNote
    {
        private const int RedirectDelaySeconds = 1;
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Parameter] public int PatientId { get; set; }
        [Parameter] public string NoteId { get; set; }
        private NoteInputModel InputModel { get; set; }
        private APIOperationStatus OperationStatus { get; set; } = APIOperationStatus.Initial;
        private ComponentMode Mode { get; set; }

        /// <summary>
        /// Component initialisation logic. 
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            SetComponentMode();
            
            if (Mode == ComponentMode.Create)
            {
                InputModel = new NoteInputModel { PatientId = PatientId };
                
                OperationStatus = APIOperationStatus.GET_Success;
                StateHasChanged();                
            }
            else
            {
                OperationStatus = APIOperationStatus.GET_Pending;
                StateHasChanged();
                
                await GetNote();
         
                OperationStatus = APIOperationStatus.GET_Success;
                StateHasChanged();      
            }
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo($"patient/{PatientId}");
        }
        
        /// <summary>
        /// Attempts to fetch a given note from the History Service. 
        /// </summary>
        /// <returns></returns>
        private async Task GetNote()
        {
            try
            {
                var response = await HttpClient.GetAsync($"http://localhost:8082/api/history/note/{NoteId}");

                if ((int) response.StatusCode == 200)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<NoteInputModel>(stringContent);

                    InputModel = content;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                OperationStatus = APIOperationStatus.GET_Error;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Set the component mode based on the URI.
        /// </summary>
        private void SetComponentMode()
        {
            if (NavigationManager.Uri.Contains("Edit", StringComparison.InvariantCultureIgnoreCase))
            {
                Mode = ComponentMode.Edit;
            }
            else
            {
                Mode = ComponentMode.Create;
            }
        }

        /// <summary>
        /// Based on the component mode, make the appropriate request to the API.
        /// </summary>
        /// <returns></returns>
        private async Task Submit()
        {
            try
            {
                if (Mode == ComponentMode.Create)
                {
                    await SubmitCreate();    
                }
                else
                {
                    await SubmitEdit();
                }
                
            }
            catch (Exception e)
            {
                OperationStatus = APIOperationStatus.POST_Error;
                StateHasChanged();
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Submits a PUT request to the API.
        /// </summary>
        /// <returns></returns>
        private async Task SubmitEdit()
        {
            OperationStatus = APIOperationStatus.PUT_Pending;
            StateHasChanged();

            var response = await HttpClient.PutAsJsonAsync($"http://localhost:8082/api/history/note/{NoteId}", InputModel);

            if (response.IsSuccessStatusCode)
            {
                OperationStatus = APIOperationStatus.PUT_Success;
                StateHasChanged();
                
                await Task.Delay(RedirectDelaySeconds * 1000);

                NavigationManager.NavigateTo($"Patient/{PatientId}/History/Note/{NoteId}");
            }
            else
            {
                OperationStatus = APIOperationStatus.POST_Error;
                StateHasChanged();
            }
        }
        
        /// <summary>
        /// Submits a POST request to the API.
        /// </summary>
        /// <returns></returns>
        private async Task SubmitCreate()
        {
            OperationStatus = APIOperationStatus.POST_Pending;
            StateHasChanged();

            var response = await HttpClient.PostAsJsonAsync("http://localhost:8082/api/history/note", InputModel);

            if (response.IsSuccessStatusCode)
            {
                var stringContent = await response.Content.ReadAsStringAsync();

                var content = JsonConvert.DeserializeObject<NoteInputModel>(stringContent);

                OperationStatus = APIOperationStatus.POST_Success;
                StateHasChanged();

                await Task.Delay(RedirectDelaySeconds * 1000);

                NavigationManager.NavigateTo($"patient/{PatientId}/history/note/{content.Id}");
            }
            else
            {
                OperationStatus = APIOperationStatus.POST_Error;
                StateHasChanged();
            }
        }
    }
}