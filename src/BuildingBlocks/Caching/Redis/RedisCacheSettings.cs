namespace Caching.Redis;

public class RedisCacheSettings
{
    public const string SectionName = "RedisCacheSettings";
    public string ConnectionString { get; set; }
    public string InstanceName { get; set; }
    public int DatabaseIndex { get; set; } = 0;
}