using FluentValidation;
using PlayTogether.Core.Dtos.Incoming.Auth;

namespace PlayTogether.Core.Dtos.Validators.Auth
{
    public class RegisterUserInfoRequestValidator: AbstractValidator<RegisterUserInfoRequest>
    {
        public RegisterUserInfoRequestValidator()
        {
            RuleFor(x => x.Firstname)
                .NotEmpty().WithMessage("Firstname is required")
                .MaximumLength(50).WithMessage("Firstname must less than 50 characters");

            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage("Lastname is required")
                .MaximumLength(50).WithMessage("Lastname must less than 50 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required");

            RuleFor(x => x.ConfirmEmail)
                .Equal(true).WithMessage("Email is not confirm");
        }
    }
}
