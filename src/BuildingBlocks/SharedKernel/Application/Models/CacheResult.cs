namespace SharedKernel.Application;

public class CacheResult<T>
{
    public string Key { get; set; }
    public T Value { get; set; }

    public CacheResult(string key, T value)
    {
        Key = key;
        Value = value;
    }
}

public class CacheResult : CacheResult<object>
{
    public CacheResult(string key, object value) : base(key, value)
    {
    }
}