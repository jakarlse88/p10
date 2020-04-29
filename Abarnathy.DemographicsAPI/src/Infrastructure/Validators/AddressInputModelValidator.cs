using System.Text.RegularExpressions;
using Abarnathy.DemographicsAPI.Models;
using FluentValidation;

namespace Abarnathy.DemographicsAPI.Infrastructure.Validators
{
    public class AddressInputModelValidator : AbstractValidator<AddressDTO>
    {
        public AddressInputModelValidator()
        {
            RuleFor(x => x.StreetName)
                .NotEmpty()
                .Matches(new Regex(@"^[^-\s][a-zA-Z ]+$"))
                .MaximumLength(40);

            RuleFor(x => x.HouseNumber)
                .NotEmpty()
                .Matches(new Regex("^[0-9]*$"))
                .WithMessage("House number can only contain numeric characters.")
                .MaximumLength(6);
            
            RuleFor(x => x.Town)
                .NotEmpty()
                .Matches(new Regex(@"^[^-\s][a-zA-Z ]+$"))
                .MaximumLength(40);
            
            RuleFor(x => x.State)
                .NotEmpty()
                .Matches(new Regex(@"^[^-\s][a-zA-Z ]+$"))
                .MaximumLength(20);

            RuleFor(x => x.Zipcode)
                .NotEmpty()
                .Matches(new Regex(@"^\d{5}(?:[-\s]\d{4})?$"));
            

        }
    }
}