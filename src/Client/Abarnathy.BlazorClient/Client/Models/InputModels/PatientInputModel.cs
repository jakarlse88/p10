using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Abarnathy.BlazorClient.Client.Models
{
    public class PatientInputModel
    {
        public PatientInputModel()
        {
            Addresses = new List<AddressInputModel>();
            
            PhoneNumbers = new List<PhoneNumberInputModel>();
            
            DateOfBirth = DateTime.Today;
            Sex = SexEnum.Default;
        }

        public int Id { get; set; }

        [Required]
        [Range((int) SexEnum.Male, (int) SexEnum.Female, ErrorMessage = "Sex must be either 'Male' or 'Female'.")]
        public SexEnum Sex { get; set; }
        
        public int SexId { get; set; }
        
        [Required(ErrorMessage = "Given name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        public string GivenName { get; set; }
        
        [Required(ErrorMessage = "Family name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        public string FamilyName { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        public List<AddressInputModel> Addresses { get; set; }
        public List<PhoneNumberInputModel> PhoneNumbers { get; set; }
    }
}
