using FluentValidation;
using PlayTogether.Core.Dtos.Incoming.Auth;

namespace PlayTogether.Core.Dtos.Validators.Auth
{
    public class RegisterAdminInfoRequestValidator:AbstractValidator<RegisterAdminInfoRequest>
    {
        public RegisterAdminInfoRequestValidator()
        {
            RuleFor(x => x.Firstname)
                .NotEmpty().WithMessage("Firstname is required")
                .MaximumLength(50).WithMessage("Firstname must less than 50 characters");

            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage("Lastname is required")
                .MaximumLength(50).WithMessage("Lastname must less than 50 characters");
        }
    }
}
