using Microsoft.Extensions.Configuration;
using Serilog;

namespace SharedKernel.Core;

public static class CoreSettings
{
    public static readonly bool IsSingleDevice = false;
    public static Dictionary<string, string> ConnectionStrings { get; private set; }
    public static DefaultLoggingConfig DefaultLoggingConfig { get; private set; }
    public static DefaultEmailConfig DefaultEmailConfig { get; private set; }
    public static DefaultElasticSearchConfig DefaultElasticSearchConfig { get; private set; }
    public static DefaultJwtConfig DefaultJwtConfig { get; private set; }
    
    public static void SetConfig(IConfiguration configuration, ILogger logger)
    {
        // SetConnectionStrings(configuration);
        // SetJwtConfig(configuration);
        // SetLoggingConfig(configuration, logger);
        // SetEmailConfig(configuration);
        // SetDefaultElasticSearchConfig(configuration);
        // SetGoogleConfig(configuration);
    }
    
    public static void SetConnectionStrings(IConfiguration configuration)
    {
        ConnectionStrings = configuration.GetRequiredSection("ConnectionStrings").Get<Dictionary<string, string>>();
    }
    
    public static void SetJwtConfig(IConfiguration configuration)
    {
        DefaultJwtConfig.SetDefaultJwtConfig(configuration);
    }

    public static void SetGoogleConfig(IConfiguration configuration)
    {
        DefaultGoogleConfig.SetDefaultGoogleConfig(configuration);
    }

    public static void SetEmailConfig(IConfiguration configuration)
    {
        DefaultEmailConfig.SetDefaultEmailConfig(configuration);
    }

    public static void SetLoggingConfig(IConfiguration configuration, ILogger logger)
    {
        DefaultLoggingConfig.SetDefaultLoggingConfig(configuration, logger);
    }
    
    public static void SetDefaultElasticSearchConfig(IConfiguration configuration)
    {
         DefaultElasticSearchConfig.SetDefaultElasticSearchConfig(configuration);
    }

    public static void SetS3AmazonConfig(IConfiguration configuration)
    {
        // DefaultS3Config.SetS3Config(configuration);
    }
}