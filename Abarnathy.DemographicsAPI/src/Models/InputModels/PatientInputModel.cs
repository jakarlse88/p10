using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public class PatientInputModel
    {
        public PatientInputModel()
        {
            Addresses = new HashSet<AddressInputModel>();
            PhoneNumbers = new HashSet<PhoneNumberInputModel>();
        }

        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }


        public int SexId { get; set; }
        
        public ICollection<AddressInputModel> Addresses { get; set; }
        public ICollection<PhoneNumberInputModel> PhoneNumbers { get; set; }
    }
}
