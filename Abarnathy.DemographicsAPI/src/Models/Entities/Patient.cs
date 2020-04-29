using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class Patient : EntityBase
    {
        public Patient()
        {
            PatientAddresses = new HashSet<PatientAddress>();
            PatientPhoneNumbers = new HashSet<PatientPhoneNumber>();
        }

        //public new int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int SexId { get; set; }
        public Sex Sex { get; set; }
        
        public ICollection<PatientAddress> PatientAddresses { get; set; }
        public ICollection<PatientPhoneNumber> PatientPhoneNumbers { get; set; }
    }
}
