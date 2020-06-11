using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Abarnathy.BlazorClient.Client.Models
{
    public class PatientViewModel
    {
        public PatientViewModel()
        {
            Addresses = new HashSet<AddressInputModel>();
            PhoneNumbers = new HashSet<PhoneNumberInputModel>();
            DateOfBirth = DateTime.Today;
        }

        public int Id { get; set; }
        
        public int SexId { get; set; }
        
        public string GivenName { get; set; }
        
        public string FamilyName { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        

        public ICollection<AddressInputModel> Addresses { get; set; }
        public ICollection<PhoneNumberInputModel> PhoneNumbers { get; set; }
    }
}