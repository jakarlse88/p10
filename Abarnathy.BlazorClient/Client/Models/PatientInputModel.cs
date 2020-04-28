using System;
using System.Collections.Generic;

namespace Abarnathy.BlazorClient.Client.Models
{
    public class PatientInputModel
    {
        public PatientInputModel()
        {
            Addresses = new HashSet<AddressInputModel>();
        }

        public int Id { get; set; }
        public int SexId { get; set; }
        // public SexEnum Sex { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<AddressInputModel> Addresses { get; set; }
    }
}
