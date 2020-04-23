using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class TelephoneNumberInputModel
    {
        public TelephoneNumberInputModel()
        {
            PatientTelephoneNumber = new HashSet<PatientTelephoneNumberInputModel>();
        }

        public int Id { get; set; }
        public string Number { get; set; }

        public ICollection<PatientTelephoneNumberInputModel> PatientTelephoneNumber { get; set; }
    }
}
