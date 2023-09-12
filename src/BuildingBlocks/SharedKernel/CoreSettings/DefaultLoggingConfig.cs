using Microsoft.Extensions.Configuration;
using Serilog;

namespace SharedKernel.Core;

public class DefaultLoggingConfig
{
    public static string AppName { get; private set; }
    public static ILogger Logger { get; private set; }
    
    public static void SetDefaultLoggingConfig(IConfiguration configuration, ILogger logger)
    {
        var appName = configuration.GetValue<string>("ApplicationName");
        if (!string.IsNullOrEmpty(appName))
        {
            AppName = appName;
        }
        Logger = logger;
    }
}