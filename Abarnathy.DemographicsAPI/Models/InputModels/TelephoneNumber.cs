using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class TelephoneNumber
    {
        public TelephoneNumber()
        {
            PatientTelephoneNumber = new HashSet<PatientTelephoneNumber>();
        }

        public int Id { get; set; }
        public string Number { get; set; }

        public virtual ICollection<PatientTelephoneNumber> PatientTelephoneNumber { get; set; }
    }
}
