using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public class SexInputModel
    {
        public SexInputModel()
        {
            Patient = new HashSet<Patient>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Patient> Patient { get; set; }
    }
}
