using Microsoft.Extensions.Configuration;

namespace SharedKernel.Core;

public class DefaultS3Config
{
    private static readonly string SectionName = "S3";
    public static string AccessKey { get; private set; }
    public static string SecretKey { get; private set; }
    public static string Bucket { get; private set; }
    public static string Root { get; private set; }
    public static List<string> AcceptExtensions { get; private set; }

    public static void SetDefaultS3Config(IConfiguration configuration)
    {
        AccessKey = configuration.GetRequiredSection("S3:AccessKey").Value ?? throw new ArgumentNullException(nameof(DefaultS3Config));
        SecretKey = configuration.GetRequiredSection("S3:SecretKey").Value ?? throw new ArgumentNullException(nameof(DefaultS3Config));
        Bucket = configuration.GetRequiredSection("S3:Bucket").Value ?? throw new ArgumentNullException(nameof(DefaultS3Config));
        Root = configuration.GetRequiredSection("S3:Root").Value ?? throw new ArgumentNullException(nameof(DefaultS3Config));
        AcceptExtensions = (configuration.GetRequiredSection("S3:AcceptExtensions").Value ?? throw new ArgumentNullException(nameof(DefaultS3Config))).Split(",").ToList();
    }
}