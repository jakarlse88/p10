using System;
using System.Net.Http;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public partial class Patient
    {
        [Parameter] public int Id { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        private PatientInputModel PatientModel { get; set; }
        private RiskLevel RiskLevel { get; set; }
        private PatientSingleOperationStatusEnum OperationStatus { get; set; }
        
        /// <summary>
        /// Component initialisation logic.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            PatientModel = new PatientInputModel();
            RiskLevel = RiskLevel.None;
            
            OperationStatus = PatientSingleOperationStatusEnum.GET_Pending;

            try
            {
                await FetchPatientData();
                await FetchAssessmentResult();

                OperationStatus = PatientSingleOperationStatusEnum.GET_Success;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                OperationStatus = PatientSingleOperationStatusEnum.GET_Error;
                StateHasChanged();
            }
        }

        private string RiskLevelCardClass() =>
            RiskLevel switch
            {
                RiskLevel.None => "alert alert-light text-center pl-5 pr-5",
                RiskLevel.Borderline => "alert alert-info text-center pl-5 pr-5",
                RiskLevel.InDanger => "alert alert-warning text-center pl-5 pr-5",
                RiskLevel.EarlyOnset => "alert alert-danger text-center pl-5 pr-5",
                _ => "alert alert-light text-center pl-5 pr-5"
            };

        private string RiskLevelDescriptionString() =>
            RiskLevel switch
            {
                RiskLevel.None => "none",
                RiskLevel.Borderline => "borderline",
                RiskLevel.InDanger => "in danger",
                RiskLevel.EarlyOnset => "early onset",
                _ => "none"
            };
        
        private async Task FetchAssessmentResult()
        {
            var response = await HttpClient.GetAsync($"http://localhost:8083/api/assessment/patient/{Id}");

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<AssessmentResult>(stringContent);

            RiskLevel = content.RiskLevel;
        }

        private async Task FetchPatientData()
        {
            var response = await HttpClient.GetAsync($"http://localhost:8080/api/patient/{Id}");

            if ((int) response.StatusCode == 200)
            {
                var stringContent = await response.Content.ReadAsStringAsync();

                var content = JsonConvert.DeserializeObject<PatientInputModel>(stringContent);

                PatientModel = content;

                PatientModel.Sex = content.SexId == 1 ? SexEnum.Male : SexEnum.Female;
            }
        }

    }
}