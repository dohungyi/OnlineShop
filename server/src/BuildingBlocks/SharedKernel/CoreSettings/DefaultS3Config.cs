using Microsoft.Extensions.Configuration;

namespace SharedKernel.Core;

public class DefaultS3Config
{ 
    public static string AccessKey { get; private set; }
    public static string SecretKey { get; private set; }
    public static string Bucket { get; private set; }
    public static string Root { get; private set; }
    public static List<string> AcceptExtensions { get; private set; }

    public static void SetDefaultS3Config(IConfiguration configuration)
    {
        AccessKey = configuration.GetRequiredSection("S3:AccessKey").Value;
        SecretKey = configuration.GetRequiredSection("S3:SecretKey").Value;
        Bucket = configuration.GetRequiredSection("S3:Bucket").Value;
        Root = configuration.GetRequiredSection("S3:Root").Value;
        AcceptExtensions = configuration.GetRequiredSection("S3:AcceptExtensions").Value.Split(",").ToList();
    }
}