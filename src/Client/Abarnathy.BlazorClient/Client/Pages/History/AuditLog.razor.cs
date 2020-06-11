using System;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.History
{
    public partial class AuditLog
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IModalService ModalService { get; set; }
        [Inject] private IJSRuntime JsRuntime { get; set; }
        [Parameter] public string PatientId { get; set; }
        [Parameter] public string NoteId { get; set; }
        private NoteInputModel NoteModel { get; set; }
        private APIOperationStatus OperationStatus { get; set; }

        private void NavigateBack()
        {
            JsRuntime.InvokeVoidAsync("NavigateBack");
        }
        
        private void ShowNoteModal(DateTime timeCreated, DateTime timeArchived, string title, string content)
        {
            var parameters = new ModalParameters();
            
            parameters.Add(nameof(NoteLogItemComponent.OriginallyCreated), timeCreated);
            parameters.Add(nameof(NoteLogItemComponent.Archived), timeArchived);
            parameters.Add(nameof(NoteLogItemComponent.Content), content);

            ModalService.Show<NoteLogItemComponent>(title, parameters);
        }
        
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

                    await JsRuntime.InvokeAsync<object>("InitDataTable", "auditlog-table");
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
    }
}