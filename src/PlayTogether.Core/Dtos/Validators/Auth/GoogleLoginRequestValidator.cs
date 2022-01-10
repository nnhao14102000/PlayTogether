using FluentValidation;
using PlayTogether.Core.Dtos.Incoming.Auth;

namespace PlayTogether.Core.Dtos.Validators.Auth
{
    public class GoogleLoginRequestValidator: AbstractValidator<GoogleLoginRequest>
    {
        public GoogleLoginRequestValidator()
        {
            RuleFor(x => x.IdToken).NotEmpty().WithMessage("Id token is required");
            RuleFor(x => x.ProviderName).NotEmpty().WithMessage("Provider name is required");
        }
    }
}
