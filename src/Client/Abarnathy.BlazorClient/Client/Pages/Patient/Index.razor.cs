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
    public partial class Index
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JsRuntime { get; set; }
        protected IEnumerable<PatientInputModel> PatientList { get; private set; }
        protected PatientsAllOperationStatusEnum StatusEnum { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            StatusEnum = PatientsAllOperationStatusEnum.Pending;

            try
            {
                var response = await HttpClient.GetAsync("http://localhost:8080/api/patient");

                if ((int)response.StatusCode == 200)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<IEnumerable<PatientInputModel>>(stringContent);

                    PatientList = content;

                    await JsRuntime.InvokeAsync<object>("InitDataTable", "patients-table");
                }

                else
                {
                    PatientList = new List<PatientInputModel>();
                    await JsRuntime.InvokeAsync<object>("InitDataTable", "patients-table");
                }

                StatusEnum = PatientsAllOperationStatusEnum.Success;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                StatusEnum = PatientsAllOperationStatusEnum.Error;
                StateHasChanged();
            }
        }
    }
}