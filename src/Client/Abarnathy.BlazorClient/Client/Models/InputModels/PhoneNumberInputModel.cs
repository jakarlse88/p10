using System;
using System.ComponentModel.DataAnnotations;

namespace Abarnathy.BlazorClient.Client.Models
{
    public class PhoneNumberInputModel
    {
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone number must conform to US format.")]
        public string Number { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Number))
            {
                throw new ArgumentNullException(nameof(Number));
            }

            var areaCode = Number.Substring(0, 3);
            var numberFirstPart = Number.Substring(3, 3);
            var numberSecondPart = Number.Substring(6, 4);

            return $"({areaCode}) {numberFirstPart}-{numberSecondPart}";
        }
    }
}