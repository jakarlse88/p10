using Abarnathy.DemographicsService.Models;
using FluentValidation;

namespace Abarnathy.DemographicsService.Infrastructure.Validators
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