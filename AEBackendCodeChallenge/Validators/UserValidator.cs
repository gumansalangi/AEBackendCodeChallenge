using AEBackendCodeChallenge.Models;
using FluentValidation;
namespace AEBackendCodeChallenge.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("User name is required.");
            RuleFor(x => x.Role).NotEmpty().WithMessage("User role is required.");
        }
    }
}
