using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class AddressInputModel
    {
        public AddressInputModel()
        {
            PatientAddress = new HashSet<PatientAddressInputModel>();
        }

        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public int Id { get; set; }

        public virtual ICollection<PatientAddressInputModel> PatientAddress { get; set; }
    }
}
