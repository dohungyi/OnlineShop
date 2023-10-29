namespace OnlineShop.Application.Features.VersionOne;

public class SetPermissionCommandValidator : AbstractValidator<SetPermissionCommand>
{
    public SetPermissionCommandValidator(IStringLocalizer<Resources> localizer)
    {
        RuleFor(r => r.Exponent)
            .NotEmpty()
            .WithMessage(localizer["auth_exponent_must_not_be_empty"].Value);
    }
}