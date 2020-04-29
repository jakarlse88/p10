using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public class PatientDTO
    {
        public PatientDTO()
        {
            Addresses = new HashSet<AddressDTO>();
            PhoneNumbers = new HashSet<PhoneNumberDTO>();
        }

        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int SexId { get; set; }
        
        public ICollection<AddressDTO> Addresses { get; set; }
        public ICollection<PhoneNumberDTO> PhoneNumbers { get; set; }
    }
}
