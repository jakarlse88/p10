using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class Address : EntityBase
    {
        public Address()
        {
            PatientAddresses = new HashSet<PatientAddress>();
        }

        public new int Id { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }

        public ICollection<PatientAddress> PatientAddresses { get; set; }
    }
}
