using Microsoft.Extensions.Configuration;

namespace SharedKernel.Core;

public class DefaultEmailConfig
{
    public static string Host { get; private set; }
    public static string AppPassword { get; private set; }
    public static int Port { get; private set; }
    public static string Sender { get; private set; }
    public static string DisplayName { get; private set; }

    public static void SetDefaultEmailConfig(IConfiguration configuration)
    {
        Host = configuration.GetRequiredSection("EmailSettings:Host").Value ?? throw new ArgumentNullException(nameof(DefaultEmailConfig));
        AppPassword = configuration.GetRequiredSection("EmailSettings:AppPassword").Value  ?? throw new ArgumentNullException(nameof(DefaultEmailConfig));
        Port = int.Parse(configuration.GetRequiredSection("EmailSettings:Port").Value ?? throw new ArgumentNullException(nameof(DefaultEmailConfig)));
        Sender = configuration.GetRequiredSection("EmailSettings:Sender").Value ?? throw new ArgumentNullException(nameof(DefaultEmailConfig));
        DisplayName = configuration.GetRequiredSection("EmailSettings:DisplayName").Value ?? throw new ArgumentNullException(nameof(DefaultEmailConfig));
    }
}