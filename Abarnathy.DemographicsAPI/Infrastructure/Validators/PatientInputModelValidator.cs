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
                .Matches(new Regex("^[0-9]*$"))
                .WithMessage("Telephone number cannot contain letters.")
                .Must(p => p.Length == 10)
                .WithMessage("Telephone number must contain exactly 10 digits.");
        }
    }
}