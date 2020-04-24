using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class Address
    {
        public Address()
        {
            PatientAddress = new HashSet<PatientAddress>();
        }

        public int Id { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }

        public ICollection<PatientAddress> PatientAddress { get; set; }
    }
}
