namespace OnlineShop.Application.Features.VersionOne;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IStringLocalizer<Resources> localizer)
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage(localizer["auth_account_must_not_be_empty"].Value);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(localizer["auth_password_must_not_be_empty"].Value);
        
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage(localizer["auth_confirm_password_must_not_be_empty"].Value);
        
        RuleFor(x => x)
            .Must(x => x.Password.Equals(x.ConfirmPassword))
            .WithMessage(localizer["auth_pwd_n_cpwd_must_same"].Value);
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localizer["auth_email_must_not_be_empty"].Value);
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer["auth_phone_must_not_be_empty"].Value);
        
        RuleFor(x => x)
            .Must(x => Utility.IsEmail(x.Email))
            .WithMessage(localizer["auth_email_is_invalid"].Value);
    }
}