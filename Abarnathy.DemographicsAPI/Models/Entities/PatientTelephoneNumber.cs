using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class PatientTelephoneNumber
    {
        public int PatientId { get; set; }
        public int TelephoneNumberId { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual TelephoneNumber TelephoneNumber { get; set; }
    }
}
