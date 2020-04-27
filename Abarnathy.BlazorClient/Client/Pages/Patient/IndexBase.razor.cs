using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public class IndexBase : ComponentBase
    {
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public IJSRuntime JsRuntime { get; set; }
        public IEnumerable<PatientInputModel> PatientList { get; set; }
        protected OperationStatus Status { get; set; }

        protected enum OperationStatus
        {
            Loading,
            Success,
            Error
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InvokeAsync<object>("InitDataTable");
                StateHasChanged();
            }
            
            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {
            Status = OperationStatus.Loading;

            try
            {
                var response = await HttpClient.GetAsync("http://localhost:8080/api/patient");

                if ((int)response.StatusCode == 200)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<IEnumerable<PatientInputModel>>(stringContent);

                    PatientList = content;
                }

                if ((int) response.StatusCode == 204)
                {
                    PatientList = new List<PatientInputModel>();
                }

                Status = OperationStatus.Success;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Status = OperationStatus.Error;
                StateHasChanged();
            }
        }
    }
}