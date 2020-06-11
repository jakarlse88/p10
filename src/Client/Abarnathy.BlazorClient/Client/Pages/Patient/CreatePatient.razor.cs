using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public partial class CreatePatient
    {
        private const int RedirectDelaySeconds = 1;
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime JsRuntime { get; set; }
        [Parameter] public string PatientId { get; set; }
        private PatientInputModel PatientModel { get; set; }
        private EditContext PatientEditContext { get; set; }
        private EditContext AddressEditContext { get; set; }
        private EditContext PhoneNumberEditContext { get; set; }
        private bool PatientValid { get; set; }
        private bool AddressValid { get; set; }
        private bool PhoneNumberValid { get; set; }
        private ComponentMode Mode { get; set; }
        private APIOperationStatus OperationStatus { get; set; }

        private bool PostPhoneNumber =>
            !string.IsNullOrWhiteSpace(PatientModel.PhoneNumbers.First().Number);

        public bool FormValid { get; set; }

        private bool PostAddress
        {
            get
            {
                var address = PatientModel.Addresses.First();

                return
                    !string.IsNullOrWhiteSpace(address.StreetName) ||
                    !string.IsNullOrWhiteSpace(address.HouseNumber) ||
                    !string.IsNullOrWhiteSpace(address.Town) ||
                    !string.IsNullOrWhiteSpace(address.State) ||
                    !string.IsNullOrWhiteSpace(address.ZipCode);
            }
        }

        /// <summary>
        /// Component initialisation logic.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            SetComponentMode();

            if (Mode == ComponentMode.Create)
            {
                InitCreate();
            }
            else
            {
                await InitEdit();
            }
            
            PatientEditContext = new EditContext(PatientModel);
            PatientEditContext.OnFieldChanged += HandlePatientOnFieldChanged;

            AddressEditContext = new EditContext(PatientModel.Addresses.First());
            AddressEditContext.OnFieldChanged += HandleAddressOnFieldChanged;

            PhoneNumberEditContext = new EditContext(PatientModel.PhoneNumbers.First());
            PhoneNumberEditContext.OnFieldChanged += HandlePhoneNumberOnFieldChanged;

            if (Mode == ComponentMode.Edit)
            {
                FormValid = true;
                PatientValid = true;
                AddressValid = true;
                PhoneNumberValid = true;
                OperationStatus = APIOperationStatus.GET_Success;
                StateHasChanged();
            }
        }

        private async Task InitEdit()
        {
            OperationStatus = APIOperationStatus.GET_Pending;

            try
            {
                var response = await HttpClient.GetAsync($"http://localhost:8080/api/patient/{PatientId}");
                response.EnsureSuccessStatusCode();

                if ((int) response.StatusCode == 200)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<PatientInputModel>(stringContent);

                    if (!content.Addresses.Any())
                    {
                        content.Addresses.Add(new AddressInputModel());
                    }
                    
                    if(!content.PhoneNumbers.Any())
                    {
                        content.PhoneNumbers.Add(new PhoneNumberInputModel());
                    } 
                    
                    PatientModel = content;
                    
                    PatientModel.Sex = content.SexId == 1 ? SexEnum.Male : SexEnum.Female;
                }
            }
            catch (Exception e)
            {
                OperationStatus = APIOperationStatus.GET_Error;
                Console.WriteLine(e);
                StateHasChanged();
            }
        }

        private void InitCreate()
        {
            FormValid = false;
            PatientValid = false;
            AddressValid = true;
            PhoneNumberValid = true;

            OperationStatus = APIOperationStatus.Initial;

            PatientModel = new PatientInputModel();
            PatientModel.Addresses.Add(new AddressInputModel());
            PatientModel.PhoneNumbers.Add(new PhoneNumberInputModel());
        }

        /// <summary>
        /// Set the component mode based on the URI.
        /// </summary>
        private void SetComponentMode()
        {
            if (NavigationManager.Uri.Contains("Edit", StringComparison.OrdinalIgnoreCase))
            {
                Mode = ComponentMode.Edit;
            }
            else
            {
                Mode = ComponentMode.Create;
            }
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
        /// Submit the Patient PUT request. 
        /// </summary>
        /// <returns></returns>
        private async Task SubmitUpdate()
        {
            OperationStatus = APIOperationStatus.PUT_Pending;
            
            StateHasChanged();

            PatientModel.SexId = (int) PatientModel.Sex;

            if (!PostAddress)
            {
                PatientModel.Addresses = new List<AddressInputModel>();
            }

            if (!PostPhoneNumber)
            {
                PatientModel.PhoneNumbers = new List<PhoneNumberInputModel>();
            }
            
            try
            {
                var response = await HttpClient.PutAsJsonAsync($"http://localhost:8080/api/patient/{PatientId}", PatientModel);

                if (response.IsSuccessStatusCode)
                {
                    OperationStatus = APIOperationStatus.PUT_Success;
                    StateHasChanged();
                    
                    await Task.Delay(RedirectDelaySeconds * 1000);

                    NavigationManager.NavigateTo($"/patient/{PatientId}");
                }
                else
                {
                    OperationStatus = APIOperationStatus.PUT_Error;
                    StateHasChanged();
                }
            }
            catch (Exception e)
            {
                OperationStatus = APIOperationStatus.PUT_Error;
                StateHasChanged();
                Console.WriteLine(e);
            }
        }
        
        /// <summary>
        /// Submit the Patient POST request. 
        /// </summary>
        /// <returns></returns>
        private async Task SubmitCreate()
        {
            OperationStatus = APIOperationStatus.POST_Pending;
            StateHasChanged();

            PatientModel.SexId = (int) PatientModel.Sex;

            if (!PostAddress)
            {
                PatientModel.Addresses = new List<AddressInputModel>();
            }

            if (!PostPhoneNumber)
            {
                PatientModel.PhoneNumbers = new List<PhoneNumberInputModel>();
            }

            try
            {
                var response = await HttpClient.PostAsJsonAsync("http://localhost:8080/api/patient", PatientModel);

                if (response.IsSuccessStatusCode)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<PatientViewModel>(stringContent);

                    OperationStatus = APIOperationStatus.POST_Success;
                    StateHasChanged();

                    await Task.Delay(RedirectDelaySeconds * 1000);

                    NavigationManager.NavigateTo($"/patient/{content.Id}");
                }
                else
                {
                    OperationStatus = APIOperationStatus.POST_Error;
                    StateHasChanged();
                }
            }
            catch (Exception e)
            {
                OperationStatus = APIOperationStatus.POST_Error;
                StateHasChanged();
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Cancels user creation and navigates back to the overview.
        /// </summary>
        private void Cancel()
        {
            // NavigationManager.NavigateTo("/");
            JsRuntime.InvokeAsync<object>("NavigateBack");
        }
    }
}