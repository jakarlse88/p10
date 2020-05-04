using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Abarnathy.BlazorClient.Client.Models
{
    public class PatientInputModel
    {
        public PatientInputModel()
        {
            Addresses = new HashSet<AddressInputModel>();
            PhoneNumbers = new HashSet<PhoneNumberInputModel>();
            DateOfBirth = DateTime.Today;
        }

        public int Id { get; set; }

        public SexEnum Sex { get; set; }
        
        // [Required]
        // [Range(1, 2)]
        public int SexId { get; set; }
        
        
        [Required]
        [MaxLength(50)]
        public string GivenName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string FamilyName { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        

        public ICollection<AddressInputModel> Addresses { get; set; }
        public ICollection<PhoneNumberInputModel> PhoneNumbers { get; set; }
    }
}
