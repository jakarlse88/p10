using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class Patient
    {
        public Patient()
        {
            PatientAddress = new HashSet<PatientAddress>();
        }

        public int Id { get; set; }
        public int SexId { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Sex Sex { get; set; }
        public virtual ICollection<PatientAddress> PatientAddress { get; set; }
    }
}
