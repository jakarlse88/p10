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
        }
    }
}