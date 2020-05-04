using System.ComponentModel.DataAnnotations;

namespace Abarnathy.BlazorClient.Client.Models
{
    public class PhoneNumberInputModel
    {
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone number must be in standard US format.")]
        // [Phone]
        public string Number { get; set; }
    }
}