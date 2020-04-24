using System;
using System.Collections.Generic;

namespace Abarnathy.DemographicsAPI.Models
{
    public partial class PatientInputModel
    {
        public PatientInputModel()
        {
            PatientAddress = new HashSet<PatientAddressInputModel>();
        }

        public int Id { get; set; }
        public int SexId { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string PhoneNumber { get; set; }

        public virtual SexInputModel Sex { get; set; }
        public virtual ICollection<PatientAddressInputModel> PatientAddress { get; set; }
    }
}
