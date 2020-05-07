using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public partial class PatientsAll
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JsRuntime { get; set; }
        protected IEnumerable<PatientInputModel> PatientList { get; private set; }

        protected OperationStatus Status { get; private set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {
            Status = OperationStatus.Pending;

            try
            {
                var response = await HttpClient.GetAsync("http://localhost:8080/api/patient");

                if ((int)response.StatusCode == 200)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<IEnumerable<PatientInputModel>>(stringContent);

                    PatientList = content;

                    await JsRuntime.InvokeAsync<object>("InitDataTable");
                }

                if ((int) response.StatusCode == 204)
                {
                    PatientList = new List<PatientInputModel>();
                    await JsRuntime.InvokeAsync<object>("InitDataTable");
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

        // public void Dispose()
        // {
        //     var runTime = JsRuntime as IJSInProcessRuntime;
        //     runTime?.Invoke<object>("DestroyDataTable");
        // }
    }
}