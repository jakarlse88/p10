using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class Patient
    {
        public Patient()
        {
            PatientAddress = new HashSet<PatientAddress>();
            PatientTelephoneNumber = new HashSet<PatientTelephoneNumber>();
        }

        public int Id { get; set; }
        public int SexId { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string FamilyName { get; set; }

        public virtual Sex Sex { get; set; }
        public virtual ICollection<PatientAddress> PatientAddress { get; set; }
        public virtual ICollection<PatientTelephoneNumber> PatientTelephoneNumber { get; set; }
    }
}
