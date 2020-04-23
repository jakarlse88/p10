using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class PatientTelephoneNumberInputModel
    {
        public int PatientId { get; set; }
        public int TelephoneNumberId { get; set; }

        public PatientInputModel Patient { get; set; }
        public TelephoneNumberInputModel TelephoneNumber { get; set; }
    }
}
