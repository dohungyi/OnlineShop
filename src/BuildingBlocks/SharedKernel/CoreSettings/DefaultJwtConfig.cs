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
        Key = configuration.GetRequiredSection("JwtSettings:Key").Value;
        Issuer = configuration.GetRequiredSection("JwtSettings:Issuer").Value;
        Audience = configuration.GetRequiredSection("JwtSettings:Audience").Value;
        ExpiredSecond = int.Parse(configuration.GetRequiredSection("JwtSettings:ExpiredSecond").Value);
    }
}