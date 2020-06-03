using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Abarnathy.BlazorClient.Client.Models
{
    public class PatientInputModel
    {
        public PatientInputModel()
        {
            Addresses = new []
            {
                new AddressInputModel()
            };
            
            PhoneNumbers = new []
            {
                new PhoneNumberInputModel()
            };
            
            DateOfBirth = DateTime.Today;
            Sex = SexEnum.Default;
        }

        public int Id { get; set; }

        [Required]
        [Range((int) SexEnum.Male, (int) SexEnum.Female, ErrorMessage = "Sex must be either 'Male' or 'Female'.")]
        public SexEnum Sex { get; set; }
        
        public int SexId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string GivenName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string FamilyName { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        public AddressInputModel[] Addresses { get; set; }
        public PhoneNumberInputModel[] PhoneNumbers { get; set; }
    }
}
