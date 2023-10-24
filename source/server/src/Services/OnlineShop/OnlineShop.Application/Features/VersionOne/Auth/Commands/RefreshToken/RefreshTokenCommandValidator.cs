

namespace OnlineShop.Application.Features.VersionOne;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator(IStringLocalizer<Resources> localizer)
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(localizer["auth_userid_must_not_be_empty"].Value);
        
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage(localizer["auth_refresh_token_must_not_be_empty"].Value);
    }
}