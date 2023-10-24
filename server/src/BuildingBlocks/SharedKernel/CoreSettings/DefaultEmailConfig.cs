using Microsoft.Extensions.Configuration;

namespace SharedKernel.Core;

public class DefaultEmailConfig
{
    public static string DisplayName { get; private set; }
    public static bool EnableVerification { get; private set; }
    public static string From { get; private set; }
    public static string SMTPServer { get; private set; }
    public static bool UseSsl { get; private set; }
    public static int Port { get; private set; }
    public static string UserName { get; private set; }
    public static string Password { get; private set; }

    public static void SetDefaultEmailConfig(IConfiguration configuration)
    {
        DisplayName = configuration.GetRequiredSection("EmailSettings:DisplayName").Value;
        EnableVerification = bool.Parse(configuration.GetRequiredSection("EmailSettings:EnableVerification").Value);
        From = configuration.GetRequiredSection("EmailSettings:From").Value;
        SMTPServer = configuration.GetRequiredSection("EmailSettings:SMTPServer").Value ;
        Port = int.Parse(configuration.GetRequiredSection("EmailSettings:Port").Value);
        UseSsl = bool.Parse(configuration.GetRequiredSection("EmailSettings:UseSsl").Value);
        UserName = configuration.GetRequiredSection("EmailSettings:UserName").Value;
        Password = configuration.GetRequiredSection("EmailSettings:Password").Value;
    }
}