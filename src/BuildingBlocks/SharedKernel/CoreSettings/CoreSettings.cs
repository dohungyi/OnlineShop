using Microsoft.Extensions.Configuration;
using Serilog;

namespace SharedKernel.CoreSettings;

public static class CoreSettings
{
    public static readonly bool IsSingleDevice = false;
    public static Dictionary<string, string> ConnectionStrings { get; private set; }
    public static DefaultLoggingConfig DefaultLoggingConfig { get; private set; }
    public static DefaultEmailConfig DefaultEmailConfig { get; private set; }
    
    public static void SetConfig(IConfiguration configuration, ILogger logger)
    {
        SetConnectionStrings(configuration);
        SetLoggingConfig(configuration, logger);
        SetEmailConfig(configuration);
    }
    
    public static void SetConnectionStrings(IConfiguration configuration)
    {
        ConnectionStrings = configuration.GetRequiredSection("ConnectionStrings").Get<Dictionary<string, string>>();
    }
    
    public static void SetEmailConfig(IConfiguration configuration)
    {
        DefaultEmailConfig.SetDefaultEmailConfig(configuration);
    }

    public static void SetLoggingConfig(IConfiguration configuration, ILogger logger)
    {
        DefaultLoggingConfig.SetDefaultLoggingConfig(configuration, logger);
    }
}