using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Abarnathy.BlazorClient.Client.Models
{
    public class AddressInputModel
    {
        private bool ShouldValidate =>
            !string.IsNullOrWhiteSpace(StreetName) ||
            !string.IsNullOrWhiteSpace(HouseNumber) ||
            !string.IsNullOrWhiteSpace(Town) ||
            !string.IsNullOrWhiteSpace(State) ||
            !string.IsNullOrWhiteSpace(ZipCode);

        [Required, MaxLength(40, ErrorMessage = "Maximum 40 characters.")]
        [RegularExpression(@"^[^-\s][a-zA-Z. ]+$", ErrorMessage = "Invalid format.")]
        [ConditionalValidation(nameof(ShouldValidate), true)]
        public string StreetName { get; set; }
        
        [Required, MaxLength(8, ErrorMessage = "Maximum 8 characters.")]
        [RegularExpression(@"^[0-9a-zA-Z ]*$", ErrorMessage = "Invalid format.")]
        [ConditionalValidation(nameof(ShouldValidate), true)]
        public string HouseNumber { get; set; }
        
        [Required, MaxLength(40, ErrorMessage = "Maximum 40 characters.")]
        [RegularExpression(@"^[^-\s][a-zA-Z ]+$", ErrorMessage = "Invalid format.")]
        [ConditionalValidation(nameof(ShouldValidate), true)]
        public string Town { get; set; }
        
        [Required, MaxLength(20, ErrorMessage = "Maximum 20 characters.")]
        [RegularExpression(@"^[^-\s][a-zA-Z ]+$", ErrorMessage = "Invalid format.")]
        [ConditionalValidation(nameof(ShouldValidate), true)]
        public string State { get; set; }
        
        [Required]
        [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Invalid format.")]
        [ConditionalValidation(nameof(ShouldValidate), true)]
        public string ZipCode { get; set; }

        public override string ToString()
        {
            return $"{StreetName} {HouseNumber}, {Town}";
        }
    }
}
