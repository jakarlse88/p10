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
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public partial class CreatePatient
    {
        private const int RedirectDelaySeconds = 5;
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        private PatientInputModel PatientModel { get; set; }
        private AddressInputModel AddressModel { get; set; }
        private List<AddressInputModel> AddedAddressModels { get; set; }
        private PhoneNumberInputModel PhoneNumberModel { get; set; }
        private List<PhoneNumberInputModel> AddedPhoneNumbers { get; set; }
        private EditContext PatientEditContext { get; set; }
        private EditContext AddressEditContext { get; set; }
        private EditContext PhoneNumberEditContext { get; set; }
        private bool PatientValid { get; set; }
        private bool CurrentAddressValid { get; set; }
        private bool CurrentPhoneNumberValid { get; set; }
        private OperationStatus OperationStatus { get; set; }

        /// <summary>
        /// Component initialisation logic.
        /// </summary>
        protected override void OnInitialized()
        {
            PatientValid = false;
            CurrentAddressValid = false;
            CurrentPhoneNumberValid = false;
            
            OperationStatus = OperationStatus.Initial;
            
            PatientModel = new PatientInputModel();
            AddressModel = new AddressInputModel();
            AddedAddressModels = new List<AddressInputModel>();
            PhoneNumberModel = new PhoneNumberInputModel();
            AddedPhoneNumbers = new List<PhoneNumberInputModel>();
            
            PatientEditContext = new EditContext(PatientModel);
            PatientEditContext.OnFieldChanged += (sender, @event) =>
            {
                PatientValid = PatientEditContext.Validate();
                StateHasChanged();
            };
            
            AddressEditContext = new EditContext(AddressModel);
            AddressEditContext.OnFieldChanged += (sender, @event) =>
                CurrentAddressValid = AddressEditContext.Validate();
            
            PhoneNumberEditContext = new EditContext(PhoneNumberModel);
            PhoneNumberEditContext.OnFieldChanged += (sender, @event) =>
                CurrentPhoneNumberValid = PhoneNumberEditContext.Validate();
        }

        /// <summary>
        /// If the <see cref="PhoneNumberInputModel"/> DTO currently being edited is valid,
        /// add it to the collection to be passed to the API.
        /// </summary>
        private void AddPhoneNumber()
        {
            if (CurrentPhoneNumberValid)
            {
                AddedPhoneNumbers.Add(PhoneNumberModel);
                PhoneNumberModel = new PhoneNumberInputModel();                
            }

            CurrentPhoneNumberValid = false;

            StateHasChanged();
        }

        /// <summary>
        /// If the <see cref="AddressInputModel"/> DTO currently being edited is valid,
        /// add it to the collection to be passed to the API.
        /// </summary>
        private void AddAddress()
        {
            if (CurrentAddressValid)
            {
                AddedAddressModels.Add(AddressModel);
                AddressModel = new AddressInputModel();                
            }

            CurrentAddressValid = false;

            StateHasChanged();
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
                    
                    await Task.Delay(RedirectDelaySeconds * 1000);
                    
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

        /// <summary>
        /// Cancels user creation and navigates back to the overview.
        /// </summary>
        private void Cancel()
        {
            NavigationManager.NavigateTo("/patient");
        }
    }
}