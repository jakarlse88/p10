using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class Sex : EntityBase
    {
        public Sex()
        {
            Patients = new HashSet<Patient>();
        }

        public new int Id { get; set; }
        public string Type { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }
}
