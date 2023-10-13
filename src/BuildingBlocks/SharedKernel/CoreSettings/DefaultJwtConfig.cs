using Microsoft.Extensions.Configuration;

namespace SharedKernel.Core;

public class DefaultJwtConfig
{
    public static string Key { get; private set; }
    public static string Issuer { get; private set; }
    public static string Audience { get; private set; }
    public static int ExpiredSecond { get; private set; }
    
    public static void SetDefaultJwtConfig(IConfiguration configuration)
    {
        Key = configuration.GetRequiredSection("Auth:JwtSettings:Key").Value ?? throw new ArgumentNullException(nameof(DefaultJwtConfig));
        Issuer = configuration.GetRequiredSection("Auth:JwtSettings:Issuer").Value ?? throw new ArgumentNullException(nameof(DefaultJwtConfig));
        Audience = configuration.GetRequiredSection("Auth:JwtSettings:Audience").Value ?? throw new ArgumentNullException(nameof(DefaultJwtConfig));
        ExpiredSecond = Convert.ToInt32(configuration.GetRequiredSection("Auth:JwtSettings:ExpiredSecond").Value ?? throw new ArgumentNullException(nameof(DefaultJwtConfig)));
    }
}