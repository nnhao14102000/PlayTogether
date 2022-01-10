using FluentValidation;
using PlayTogether.Core.Dtos.Incoming.Auth;

namespace PlayTogether.Core.Dtos.Validators.Auth
{
    public class RegisterCharityInfoRequestValidator: AbstractValidator<RegisterCharityInfoRequest>
    {
        public RegisterCharityInfoRequestValidator()
        {
            RuleFor(x => x.OrganizationName)
                .NotEmpty().WithMessage("Organization name is required")
                .MaximumLength(50).WithMessage("Organization name must less than 50 character");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required");
        }
    }
}
