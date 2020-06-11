using System.Collections.Generic;

namespace Abarnathy.DemographicsService.Models
{
    public class Sex : EntityBase
    {
        public Sex()
        {
            Patients = new HashSet<Patient>();
        }

        //public new int Id { get; set; }
        public string Type { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }
}
