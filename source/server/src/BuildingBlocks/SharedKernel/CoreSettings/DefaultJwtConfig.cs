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
        Key = configuration.GetRequiredSection("Auth:JwtSettings:Key").Value;
        Issuer = configuration.GetRequiredSection("Auth:JwtSettings:Issuer").Value;
        Audience = configuration.GetRequiredSection("Auth:JwtSettings:Audience").Value;
        ExpiredSecond = int.Parse(configuration.GetRequiredSection("Auth:JwtSettings:ExpiredSecond").Value);
    }
}