using FluentValidation;
using Microsoft.Extensions.Localization;
using OnlineShop.Application.Properties;

namespace OnlineShop.Application.Features.VersionOne;

public class SignInByGoogleCommandValidator : AbstractValidator<SignInByGoogleCommand>
{
    public SignInByGoogleCommandValidator(IStringLocalizer<Resources> localizer)
    {
        RuleFor(command => command.IdToken)
            .NotEmpty()
            .WithMessage(localizer["auth_id_token_must_not_be_empty"].Value);
    }
}