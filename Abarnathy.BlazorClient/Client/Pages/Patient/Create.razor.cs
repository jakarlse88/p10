using System.Collections.Generic;
using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;

namespace Abarnathy.BlazorClient.Client.Pages.Patient
{
    public class CreateBase : ComponentBase
    {
        protected PatientInputModel Model { get; set; }
        protected HashSet<AddressInputModel> Addresses { get; set; } 
        protected SexEnum Sex { get; set; }

        protected override void OnInitialized()
        {
            Model = new PatientInputModel();
            // Addresses
        }

        protected void Submit()
        {
            // Convert from SexEnum to Id (ie. cast to int)
        }
    }
}