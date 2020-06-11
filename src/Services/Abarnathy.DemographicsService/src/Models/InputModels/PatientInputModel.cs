using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsService.Models
{
    public class PatientInputModel
    {
        public PatientInputModel()
        {
            Addresses = new List<AddressInputModel>();
            PhoneNumbers = new List<PhoneNumberInputModel>();
        }

        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int SexId { get; set; }
        
        public List<AddressInputModel> Addresses { get; set; }
        public List<PhoneNumberInputModel> PhoneNumbers { get; set; }
    }
}
