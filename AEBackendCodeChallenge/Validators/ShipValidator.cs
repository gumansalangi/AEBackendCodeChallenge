using AEBackendCodeChallenge.Models;
using FluentValidation;
namespace AEBackendCodeChallenge.Validators
{
    public class ShipValidator : AbstractValidator<Ship>
    {
        public ShipValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ship name is required.");
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
            RuleFor(x => x.Velocity).GreaterThanOrEqualTo(0).WithMessage("Velocity must be a non-negative number.");
        }
    }
}
