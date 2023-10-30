namespace OnlineShop.Application.Features.VersionOne;

public class CreateOrUpdateConfigCommand : BaseUpdateCommand<ApiResult>
{
    public ConfigValue ConfigValue { get; }

    public CreateOrUpdateConfigCommand(ConfigValue configValue)
    {
        ConfigValue = configValue;
    }
}