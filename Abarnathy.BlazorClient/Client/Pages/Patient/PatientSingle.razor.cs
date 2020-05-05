using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public partial class PatientSingle
    {
        private const int RedirectDelaySeconds = 5;
        [Parameter] public int Id { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        private PatientInputModel Model { get; set; }
        private AddressInputModel AddressModel { get; set; }
        private List<AddressInputModel> AddedAddressModels { get; set; }
        private PhoneNumberInputModel PhoneNumberModel { get; set; }
        private List<PhoneNumberInputModel> AddedPhoneNumbers { get; set; }
        private OperationStatus OperationStatus { get; set; }
        private bool EnableEdit { get; set; }

        private void ToggleEdit()
        {
            EnableEdit = !EnableEdit;
            StateHasChanged();
        }

        private void AddPhoneNumber()
        {
            AddedPhoneNumbers.Add(PhoneNumberModel);
            PhoneNumberModel = new PhoneNumberInputModel();
            StateHasChanged();
        }

        private void AddAddress()
        {
            AddedAddressModels.Add(AddressModel);
            AddressModel = new AddressInputModel();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            AddressModel = new AddressInputModel();
            AddedAddressModels = new List<AddressInputModel>();
            PhoneNumberModel = new PhoneNumberInputModel();
            AddedPhoneNumbers = new List<PhoneNumberInputModel>();
            
            OperationStatus = OperationStatus.Pending;

            try
            {
                var response = await HttpClient.GetAsync($"http://localhost:8080/api/patient/{Id}");

                if ((int) response.StatusCode == 200)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var content = JsonConvert.DeserializeObject<PatientInputModel>(stringContent);

                    Model = content;

                    if (!Model.Addresses.Any())
                    {
                        AddedAddressModels = new List<AddressInputModel>();
                        AddressModel = new AddressInputModel();
                    }
                    else
                    {

                        AddedAddressModels = Model.Addresses.ToList();

                    }

                    if (!Model.PhoneNumbers.Any())
                    {
                        AddedPhoneNumbers = new List<PhoneNumberInputModel>();
                        PhoneNumberModel = new PhoneNumberInputModel();
                    }
                    else
                    {
                        AddedPhoneNumbers = Model.PhoneNumbers.ToList();
                    }
                }

                OperationStatus = OperationStatus.Success;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                OperationStatus = OperationStatus.Error;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Submit the Patient update form. 
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
                    Model.Addresses.Add(item);
                }
            }

            if (AddedPhoneNumbers.Any())
            {
                foreach (var item in AddedPhoneNumbers)
                {
                    Model.PhoneNumbers.Add(item);
                }
            }

            Model.SexId = (int) Model.Sex;

            try
            {
                var response = await HttpClient.PostAsJsonAsync("http://localhost:8080/api/patient", Model);

                if (response.IsSuccessStatusCode)
                {
                    OperationStatus = OperationStatus.Success;
                    StateHasChanged();

                    await Task.Delay(RedirectDelaySeconds * 1000);

                    NavigationManager.NavigateTo($"/patient/{Id}");
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