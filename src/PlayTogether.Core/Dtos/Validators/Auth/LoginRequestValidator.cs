using FluentValidation;
using PlayTogether.Core.Dtos.Incoming.Auth;

namespace PlayTogether.Core.Dtos.Validators.Auth
{
    public class LoginRequestValidator: AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is invalid");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .MaximumLength(30).WithMessage("Password must be less than 30 letters");
        }
    }
}
