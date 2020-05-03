using System.Collections.Generic;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public class CreateBase : ComponentBase
    {
        protected PatientInputModel Model { get; set; }
        protected PhoneNumberInputModel PhoneNumberModel { get; set; }

        protected override void OnInitialized()
        {
            Model = new PatientInputModel();
            PhoneNumberModel = new PhoneNumberInputModel();
        }

        protected void AddPhoneNumber()
        {
            Model.PhoneNumbers.Add(PhoneNumberModel);
            PhoneNumberModel = new PhoneNumberInputModel();
        }
        
        protected void Submit()
        {
            // Convert from SexEnum to Id (ie. cast to int)
        }
    }
}