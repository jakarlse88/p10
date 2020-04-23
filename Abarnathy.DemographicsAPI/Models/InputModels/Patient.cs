using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public class PatientInputModel
    {
        public PatientInputModel()
        {
            PatientAddress = new HashSet<PatientAddress>();
            PatientTelephoneNumber = new HashSet<PatientTelephoneNumber>();
        }

        public int Id { get; set; }
        public int SexId { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string FamilyName { get; set; }

        public Sex Sex { get; set; }
        public ICollection<PatientAddress> PatientAddress { get; set; }
        public ICollection<PatientTelephoneNumber> PatientTelephoneNumber { get; set; }
    }
}
