namespace OnlineShop.Application;

public class UserConfigDto : IMapFrom<ApplicationUserConfig>
{
    public ConfigValue ConfigValue { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? LastModifiedDate { get; set; }
}