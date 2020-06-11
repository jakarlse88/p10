using System.Text.RegularExpressions;
using Abarnathy.DemographicsService.Models;
using FluentValidation;

namespace Abarnathy.DemographicsService.Infrastructure.Validators
{
    public class AddressInputModelValidator : AbstractValidator<AddressInputModel>
    {
        public AddressInputModelValidator()
        {
            RuleFor(x => x.StreetName)
                .NotEmpty()
                .Matches(new Regex(@"^[^-\s][a-zA-Z. ]+$"))
                .MaximumLength(40);

            RuleFor(x => x.HouseNumber)
                .NotEmpty()
                .Matches(new Regex(@"^[0-9a-zA-Z ]*$"))
                .MaximumLength(8);
            
            RuleFor(x => x.Town)
                .NotEmpty()
                .Matches(new Regex(@"^[^-\s][a-zA-Z ]+$"))
                .MaximumLength(40);
            
            RuleFor(x => x.State)
                .NotEmpty()
                .Matches(new Regex(@"^[^-\s][a-zA-Z ]+$"))
                .MaximumLength(20);

            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .Matches(new Regex(@"^\d{5}(?:[-\s]\d{4})?$"));
            

        }
    }
}