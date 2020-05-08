using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public partial class PatientSingle
    {
        private const int RedirectDelaySeconds = 5;
        [Parameter] public int Id { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime JSRunTime { get; set; }
        private PatientInputModel PatientModel { get; set; }
        private AddressInputModel AddressModel { get; set; }
        private List<AddressInputModel> AddedAddresses { get; set; }
        private PhoneNumberInputModel PhoneNumberModel { get; set; }
        private List<PhoneNumberInputModel> AddedPhoneNumbers { get; set; }
        private EditContext PatientEditContext { get; set; }
        private EditContext AddressEditContext { get; set; }
        private EditContext PhoneNumberEditContext { get; set; }
        private bool PatientValid { get; set; }
        private bool CurrentAddressValid { get; set; }
        private bool CurrentPhoneNumberValid { get; set; }
        private bool EnableEdit { get; set; }
        private PatientSingleOperationStatusEnum OperationStatus { get; set; }

        /// <summary>
        /// Component initialisation logic.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            PatientModel = new PatientInputModel();
            AddressModel = new AddressInputModel();
            AddedAddresses = new List<AddressInputModel>();
            PhoneNumberModel = new PhoneNumberInputModel();
            AddedPhoneNumbers = new List<PhoneNumberInputModel>();
            
            CurrentAddressValid = false;
            CurrentPhoneNumberValid = false;
            
            AddressEditContext = new EditContext(AddressModel);
            AddressEditContext.OnFieldChanged += (sender, @event) =>
                CurrentAddressValid = AddressEditContext.Validate();
            
            PhoneNumberEditContext = new EditContext(PhoneNumberModel);
            PhoneNumberEditContext.OnFieldChanged += (sender, @event) =>
                CurrentPhoneNumberValid = PhoneNumberEditContext.Validate();
            
            OperationStatus = PatientSingleOperationStatusEnum.GET;

            try
            {
                var response = await HttpClient.GetAsync($"http://localhost:8080/api/patient/{Id}");

                if ((int) response.StatusCode == 200)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<PatientInputModel>(stringContent);

                    PatientModel = content;

                    PatientModel.Sex = content.SexId == 1 ? SexEnum.Male : SexEnum.Female;
                    
                    PatientEditContext = new EditContext(PatientModel);
                    PatientEditContext.OnFieldChanged += (sender, @event) =>
                    {
                        PatientValid = PatientEditContext.Validate();
                        Console.WriteLine("Something changed");
                        StateHasChanged();
                    };
                    
                    PatientValid = PatientEditContext.Validate();
                    
                    if (PatientModel.Addresses.Any())
                    {
                        AddedAddresses = PatientModel.Addresses.ToList();
                    }

                    if (PatientModel.PhoneNumbers.Any())
                    {
                        AddedPhoneNumbers = PatientModel.PhoneNumbers.ToList();
                    }
                }

                OperationStatus = PatientSingleOperationStatusEnum.GETSuccess;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                OperationStatus = PatientSingleOperationStatusEnum.GETError;
                StateHasChanged();
            }
        }
        
        /// <summary>
        /// Toggle fields readonly/editable.
        /// </summary>
        private void ToggleEdit()
        {
            EnableEdit = !EnableEdit;
            StateHasChanged();
        }

        /// <summary>
        /// Cancels the operation and returns to the Patient overview.
        /// </summary>
        private void Cancel()
        {
         NavigationManager.NavigateTo("/patient");   
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
        /// Removes a PhoneNumber from the collection to be passed to the API.
        /// </summary>
        /// <param name="number"></param>
        private void RemovePhoneNumber(string number)
        {
            var newList = new List<PhoneNumberInputModel>();

            foreach (var item in AddedPhoneNumbers)
            {
                if (Regex.Replace(item.Number, @"[- ().]", "") != Regex.Replace(number, @"[- ().]", ""))
                {
                    newList.Add(item);
                }
            }

            AddedPhoneNumbers = newList;
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
                AddedAddresses.Add(AddressModel);
                AddressModel = new AddressInputModel();                
            }

            CurrentAddressValid = false;

            StateHasChanged();
        }

        /// <summary>
        /// Removes an address from the collection to be passed to the API.
        /// </summary>
        /// <param name="model"></param>
        private void RemoveAddress(AddressInputModel model)
        {
            var newList = new List<AddressInputModel>();

            foreach (var item in AddedAddresses)
            {
                if ((!string.Equals(item.StreetName, model.StreetName, StringComparison.CurrentCultureIgnoreCase)) &&
                    (!string.Equals(item.HouseNumber, model.HouseNumber, StringComparison.CurrentCultureIgnoreCase)) &&
                    (!string.Equals(item.Town, model.Town, StringComparison.CurrentCultureIgnoreCase)) &&
                    (!string.Equals(item.State, model.State, StringComparison.CurrentCultureIgnoreCase)) &&
                    (!string.Equals(item.ZipCode, model.ZipCode, StringComparison.CurrentCultureIgnoreCase)))
                {
                    newList.Add(item);
                }
            }

            AddedAddresses = newList;
            StateHasChanged();
        }

        /// <summary>
        /// Submit the Patient update form. 
        /// </summary>
        /// <returns></returns>
        private async Task Submit()
        {
            OperationStatus = PatientSingleOperationStatusEnum.PUT;
            
            StateHasChanged();

            PatientModel.Addresses = AddedAddresses;
            PatientModel.PhoneNumbers = AddedPhoneNumbers;
            PatientModel.SexId = (int) PatientModel.Sex;

            try
            {
                var response = await HttpClient.PutAsJsonAsync($"http://localhost:8080/api/patient/{Id}", PatientModel);

                if (response.IsSuccessStatusCode)
                {
                    OperationStatus = PatientSingleOperationStatusEnum.PUTSuccess;
                    StateHasChanged();

                    await Task.Delay(RedirectDelaySeconds * 1000);

                    await JSRunTime.InvokeVoidAsync("ForceReload");
                }
                else
                {
                    OperationStatus = PatientSingleOperationStatusEnum.PUTError;
                    StateHasChanged();
                }
            }
            catch (Exception e)
            {
                OperationStatus = PatientSingleOperationStatusEnum.PUTError;
                StateHasChanged();
                Console.WriteLine(e);
            }
        }
    }
}