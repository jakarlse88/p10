using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public partial class Create
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        private const int RedirectDelaySeconds = 5;
        private PatientInputModel PatientModel { get; set; }
        private AddressInputModel AddressModel { get; set; }
        private List<AddressInputModel> AddedAddressModels { get; set; }
        private PhoneNumberInputModel PhoneNumberModel { get; set; }
        private List<PhoneNumberInputModel> AddedPhoneNumbers { get; set; }
        private OperationStatus OperationStatus { get; set; }

        protected override void OnInitialized()
        {
            OperationStatus = OperationStatus.Initial;
            PatientModel = new PatientInputModel();
            AddressModel = new AddressInputModel();
            AddedAddressModels = new List<AddressInputModel>();
            PhoneNumberModel = new PhoneNumberInputModel();
            AddedPhoneNumbers = new List<PhoneNumberInputModel>();
        }

        /// <summary>
        /// Submit the Patient creation form. 
        /// </summary>
        /// <returns></returns>
        private async Task Submit()
        {
            OperationStatus = OperationStatus.Pending;
            StateHasChanged();

            if (AddedAddressModels.Any())
            {
                foreach (var item in AddedAddressModels)
                {
                    PatientModel.Addresses.Add(item);
                }
            }

            if (AddedPhoneNumbers.Any())
            {
                foreach (var item in AddedPhoneNumbers)
                {
                    PatientModel.PhoneNumbers.Add(item);
                }
            }

            PatientModel.SexId = (int) PatientModel.Sex;

            try
            {
                var response = await HttpClient.PostAsJsonAsync("http://localhost:8080/api/patient", PatientModel);

                if (response.IsSuccessStatusCode)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<PatientViewModel>(stringContent);
                    
                    OperationStatus = OperationStatus.Success;
                    StateHasChanged();
                    
                    // Thread.Sleep(RedirectDelaySeconds * 1000);
                    await Task.Delay(5000);
                    
                    NavigationManager.NavigateTo($"/patient/{content.Id}");
                }
                else
                {
                    OperationStatus = OperationStatus.Error;
                    StateHasChanged();
                }
            }
            catch (Exception e)
            {
                OperationStatus = OperationStatus.Error;
                StateHasChanged();
                Console.WriteLine(e);
            }
        }
    }
}