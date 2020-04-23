using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public class AddressInputModel
    {
        public AddressInputModel()
        {
            PatientAddress = new HashSet<PatientAddressInputModel>();
        }

        public int Id { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }

        public ICollection<PatientAddressInputModel> PatientAddress { get; set; }
    }
}
