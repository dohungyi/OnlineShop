using FluentValidation;
using Microsoft.Extensions.Localization;
using OnlineShop.Application.Properties;

namespace OnlineShop.Application.Features.VersionOne;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator(IStringLocalizer<Resources> localizer)
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage(localizer["auth_account_must_not_be_empty"].Value);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(localizer["auth_password_must_not_be_empty"].Value);
    }
}