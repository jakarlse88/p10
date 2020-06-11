using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Shared.Components.NoteDisplay
{
    public partial class NoteDisplay 
    {
        private const string TableName = "notes-table";
        [Parameter] public int PatientId { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JsRuntime { get; set; }
        private IEnumerable<NoteInputModel> Notes { get; set; }
        private NoteDisplayOperationStatus OperationStatus { get; set; }

        /// <summary>
        /// Component initialisation logic.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            Notes = new List<NoteInputModel>();
            OperationStatus = NoteDisplayOperationStatus.Initial;
            
            try
            {
                var response = await HttpClient.GetAsync($"http://localhost:8082/api/history/patient/{PatientId}");

                if ((int) response.StatusCode == 200)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<IEnumerable<NoteInputModel>>(stringContent);

                    Notes = content;
                }
                
                await JsRuntime.InvokeAsync<object>("InitDataTable", TableName);
                OperationStatus = NoteDisplayOperationStatus.GET_Success;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                OperationStatus = NoteDisplayOperationStatus.GET_Error;
                StateHasChanged();
            }
        }
    }
}