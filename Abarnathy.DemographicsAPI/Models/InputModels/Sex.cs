using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class SexInputModel
    {
        public SexInputModel()
        {
            Patient = new HashSet<PatientInputModel>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<PatientInputModel> Patient { get; set; }
    }
}
