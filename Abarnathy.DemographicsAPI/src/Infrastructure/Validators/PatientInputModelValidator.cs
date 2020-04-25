using System.Text.RegularExpressions;
using Abarnathy.DemographicsAPI.Models;
using FluentValidation;

namespace Abarnathy.DemographicsAPI.Infrastructure.Validators
{
    public class PatientInputModelValidator : AbstractValidator<PatientInputModel>
    {
        public PatientInputModelValidator()
        {
            RuleFor(x => x.SexId)
                .NotEmpty()
                .InclusiveBetween(1, 2);

            RuleFor(x => x.FamilyName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.GivenName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.PhoneNumber)
                .Matches(new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
                    .When(s => !string.IsNullOrWhiteSpace(s.PhoneNumber))
                .WithMessage("Telephone number must conform to US standard.");
        }
    }
}