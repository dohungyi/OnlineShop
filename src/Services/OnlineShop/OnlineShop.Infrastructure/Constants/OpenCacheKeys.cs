namespace OnlineShop.Infrastructure.Constants;

public class OpenCacheKeys
{
    public static string GetAvatarUrlKey(object userId) => $"avatar:{userId}";
}