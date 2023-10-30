namespace OnlineShop.Application.Dtos;

public class ConfigValue
{
    public string Language { get; set; } = "en-US";
}

public class ConfigValueValidator : AbstractValidator<ConfigValue>
{
    public ConfigValueValidator(IStringLocalizer<Resources> localizer)
    {
        RuleFor(x => x.Language).NotEmpty().WithMessage(localizer["config_language_must_not_be_empty"].Value);
    }
}