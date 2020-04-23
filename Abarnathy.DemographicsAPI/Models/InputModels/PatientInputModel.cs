using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public class PatientInputModel
    {
        public PatientInputModel()
        {
            PatientAddress = new HashSet<PatientAddressInputModel>();
            PatientTelephoneNumber = new HashSet<PatientTelephoneNumberInputModel>();
        }

        public int Id { get; set; }
        public int SexId { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string FamilyName { get; set; }

        public SexInputModel Sex { get; set; }
        public ICollection<PatientAddressInputModel> PatientAddress { get; set; }
        public ICollection<PatientTelephoneNumberInputModel> PatientTelephoneNumber { get; set; }
    }
}
