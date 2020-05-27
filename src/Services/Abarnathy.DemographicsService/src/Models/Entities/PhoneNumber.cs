using System.Collections.Generic;

namespace Abarnathy.DemographicsService.Models
{
    public class PhoneNumber : EntityBase
    {
        public PhoneNumber()
        {
            PatientPhoneNumbers = new HashSet<PatientPhoneNumber>();
        }
        
        //public new int Id { get; set; }
        public string Number { get; set; }

        
        public ICollection<PatientPhoneNumber> PatientPhoneNumbers { get; set; }
    }
}