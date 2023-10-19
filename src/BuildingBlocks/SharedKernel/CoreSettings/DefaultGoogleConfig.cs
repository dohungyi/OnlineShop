using Microsoft.Extensions.Configuration;

namespace SharedKernel.Core;

public class DefaultGoogleConfig
{
    public static string ClientId { get; private set; }
    public static string ClientSecret { get; private set; }
    
    public static void SetDefaultGoogleConfig(IConfiguration configuration)
    {
        ClientId = configuration.GetRequiredSection("Auth:GoogleSettings:ClientId").Value;
        ClientSecret = configuration.GetRequiredSection("Auth:GoogleSettings:ClientSecret").Value;
        
    }
}