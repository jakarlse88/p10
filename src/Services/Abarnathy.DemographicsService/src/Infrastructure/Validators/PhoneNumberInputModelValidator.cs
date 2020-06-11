using System.Text.RegularExpressions;
using Abarnathy.DemographicsService.Models;
using FluentValidation;

namespace Abarnathy.DemographicsService.Infrastructure.Validators
{
    public class PhoneNumberInputModelValidator : AbstractValidator<PhoneNumberInputModel>
    {
        public PhoneNumberInputModelValidator()
        {
            RuleFor(x => x.Number)
                .Matches(new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
                .When(s => !string.IsNullOrWhiteSpace(s.Number))
                .WithMessage("Telephone number must conform to US standard.");
        }
    }
}