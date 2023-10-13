using Microsoft.Extensions.Configuration;

namespace SharedKernel.Core;

public class DefaultElasticSearchConfig
{
    public static string Uri { get; private set; }
    public static string ApplicationName { get; private set; }
    public static string Username { get; private set; }
    public static string Password { get; private set; }

    public static void SetDefaultElasticSearchConfig(IConfiguration configuration)
    {
        ApplicationName = configuration.GetRequiredSection("ApplicationName").Value ?? throw new ArgumentNullException(nameof(DefaultElasticSearchConfig));
        Uri = configuration.GetRequiredSection("ElasticConfiguration:Uri").Value ?? throw new ArgumentNullException(nameof(DefaultElasticSearchConfig));
        Username = configuration.GetRequiredSection("ElasticConfiguration:Username").Value ?? throw new ArgumentNullException(nameof(DefaultElasticSearchConfig));
        Password = configuration.GetRequiredSection("ElasticConfiguration:Password").Value ?? throw new ArgumentNullException(nameof(DefaultElasticSearchConfig));
    }
}