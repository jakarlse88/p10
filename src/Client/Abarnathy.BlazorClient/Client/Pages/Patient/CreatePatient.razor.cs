using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public partial class CreatePatient
    {
        private const int RedirectDelaySeconds = 1;
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        private PatientInputModel PatientModel { get; set; }
        private EditContext PatientEditContext { get; set; }
        private EditContext AddressEditContext { get; set; }
        private EditContext PhoneNumberEditContext { get; set; }
        private bool PatientValid { get; set; }
        private bool AddressValid { get; set; }
        private bool PhoneNumberValid { get; set; }

        private bool PostAddress
        {
            get
            {
                var address = PatientModel.Addresses[0];
                
                return 
                    !string.IsNullOrWhiteSpace(address.StreetName) ||
                    !string.IsNullOrWhiteSpace(address.HouseNumber) ||
                    !string.IsNullOrWhiteSpace(address.Town) ||
                    !string.IsNullOrWhiteSpace(address.State) ||
                    !string.IsNullOrWhiteSpace(address.ZipCode);
            }
        }

        private bool PostPhoneNumber => 
            !string.IsNullOrWhiteSpace(PatientModel.PhoneNumbers[0].Number);
        private PatientsAllOperationStatusEnum PatientsAllOperationStatusEnum { get; set; }

        public bool FormValid { get; set; }

        /// <summary>
        /// Component initialisation logic.
        /// </summary>
        protected override void OnInitialized()
        {
            FormValid = false;
            PatientValid = false;
            AddressValid = true;
            PhoneNumberValid = true;
            
            PatientsAllOperationStatusEnum = PatientsAllOperationStatusEnum.Initial;

            PatientModel = new PatientInputModel();

            PatientEditContext = new EditContext(PatientModel);
            PatientEditContext.OnFieldChanged += HandlePatientOnFieldChanged;

            AddressEditContext = new EditContext(PatientModel.Addresses[0]);
            AddressEditContext.OnFieldChanged += HandleAddressOnFieldChanged;

            PhoneNumberEditContext = new EditContext(PatientModel.PhoneNumbers[0]);
            PhoneNumberEditContext.OnFieldChanged += HandlePhoneNumberOnFieldChanged;
        }

        private void HandlePatientOnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            PatientValid = PatientEditContext.Validate();
            UpdateFormValid();
        }

        private void HandleAddressOnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            AddressValid = AddressEditContext.Validate();
            UpdateFormValid();
        }

        private void HandlePhoneNumberOnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            PhoneNumberValid = PhoneNumberEditContext.Validate();
            UpdateFormValid();
        }

        private void UpdateFormValid()
        {
            FormValid = PatientValid &&
                        AddressValid &&
                        PhoneNumberValid;
            
            StateHasChanged();
        }

        /// <summary>
        /// Submit the Patient creation form. 
        /// </summary>
        /// <returns></returns>
        private async Task Submit()
        {
            PatientsAllOperationStatusEnum = PatientsAllOperationStatusEnum.Pending;
            StateHasChanged();

            PatientModel.SexId = (int) PatientModel.Sex;

            if (!PostAddress)
            {
                PatientModel.Addresses = new AddressInputModel[]{};
            }

            if (!PostPhoneNumber)
            {
                PatientModel.PhoneNumbers = new PhoneNumberInputModel[]{};
            }

            try
            {
                var response = await HttpClient.PostAsJsonAsync("http://localhost:8080/api/patient", PatientModel);

                if (response.IsSuccessStatusCode)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<PatientViewModel>(stringContent);

                    PatientsAllOperationStatusEnum = PatientsAllOperationStatusEnum.Success;
                    StateHasChanged();

                    await Task.Delay(RedirectDelaySeconds * 1000);

                    NavigationManager.NavigateTo($"/patient/{content.Id}");
                }
                else
                {
                    PatientsAllOperationStatusEnum = PatientsAllOperationStatusEnum.Error;
                    StateHasChanged();
                }
            }
            catch (Exception e)
            {
                PatientsAllOperationStatusEnum = PatientsAllOperationStatusEnum.Error;
                StateHasChanged();
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Cancels user creation and navigates back to the overview.
        /// </summary>
        private void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}