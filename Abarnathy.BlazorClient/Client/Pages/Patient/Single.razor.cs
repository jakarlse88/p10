using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Abarnathy.BlazorClient.Client.Shared;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public class SingleBase : ComponentBase
    {
        [Parameter] public int Id { get; set; }
        [Inject] public HttpClient HttpClient { get; set; }
        public PatientInputModel Model { get; set; } = null;
        public OperationStatus Status { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Status = OperationStatus.Pending;

            try
            {
                var response = await HttpClient.GetAsync($"http://localhost:8080/api/patient/{Id}");
                
                if ((int) response.StatusCode == 200)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<PatientInputModel>(stringContent);

                    Model = content;
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