namespace Abarnathy.DemographicsAPI.Models
{
    public class PatientPhoneNumber
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int PhoneNumberId { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
    }
}