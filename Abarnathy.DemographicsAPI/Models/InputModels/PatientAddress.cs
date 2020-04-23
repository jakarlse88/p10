using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class PatientAddressInputModel
    {
        public int PatientId { get; set; }
        public int AddressId { get; set; }

        public AddressInputModel Address { get; set; }
        public PatientInputModel Patient { get; set; }
    }
}
