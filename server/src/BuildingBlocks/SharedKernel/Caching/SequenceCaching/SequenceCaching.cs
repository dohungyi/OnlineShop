namespace SharedKernel.Caching;

public class SequenceCaching : ISequenceCaching
{
    private readonly IMemoryCaching _memCaching;
    private readonly IRedisCache _redisCaching;

    public SequenceCaching(IMemoryCaching memCaching, IRedisCache redisCaching)
    {
        _memCaching = memCaching;
        _redisCaching = redisCaching;
    }
    
    public TimeSpan DefaultAbsoluteExpireTime => TimeSpan.FromHours(2);
    
    public async Task<object> GetAsync(string key, CachingType type = CachingType.Couple, CancellationToken cancellationToken = default)
    {
        switch (type)
        {
            case CachingType.Couple:
                var result = await _memCaching.GetAsync<object>(key);
                if (result is null)
                {
                    result = await _redisCaching.GetAsync<object>(key);
                    if (result is not null)
                    {
                        await _memCaching.SetAsync(key, result, DefaultAbsoluteExpireTime);
                    }
                }
                return result;
            case CachingType.Memory:
                return await _memCaching.GetAsync<object>(key);
            case CachingType.Redis:
                return await _redisCaching.GetAsync<object>(key);
        }
        throw new Exception("The caching type is invalid. Please re-check!!!");
    }

    public async Task<T> GetAsync<T>(string key, CachingType type = CachingType.Couple, CancellationToken cancellationToken = default)
    {
        switch (type)
        {
            case CachingType.Couple:
                var result = await _memCaching.GetAsync<T>(key);
                if (result is null)
                {
                    result = await _redisCaching.GetAsync<T>(key);
                    if (result is not null)
                    {
                        await _memCaching.SetAsync(key, result, DefaultAbsoluteExpireTime);
                    }
                }
                return result;
            case CachingType.Memory:
                return await _memCaching.GetAsync<T>(key);
            case CachingType.Redis:
                return await _redisCaching.GetAsync<T>(key);
        }
        throw new Exception("The caching type is invalid. Please re-check!!!");
    }

    public async Task<string> GetStringAsync(string key, CachingType type = CachingType.Couple, CancellationToken cancellationToken = default)
    {
        switch (type)
        {
            case CachingType.Couple:
                var result = await _memCaching.GetStringAsync(key);
                if (string.IsNullOrEmpty(result))
                {
                    result = await _redisCaching.GetStringAsync(key);
                    if (!string.IsNullOrEmpty(result))
                    {
                        await _memCaching.SetAsync(key, result, DefaultAbsoluteExpireTime);
                    }
                }
                return result;
            case CachingType.Memory:
                return await _memCaching.GetStringAsync(key);
            case CachingType.Redis:
                return await _redisCaching.GetStringAsync(key);
        }
        throw new Exception("The caching type is invalid. Please re-check!!!");
    }

    public async Task SetAsync(string key, object value, TimeSpan? absoluteExpireTime = null, bool keepTtl = false , CachingType onlyUseType = CachingType.Couple, CancellationToken cancellationToken = default)
    {
        switch (onlyUseType)
        {
            case CachingType.Couple:
                await _memCaching.SetAsync(key, value, absoluteExpireTime, keepTtl);
                await _redisCaching.SetAsync(key, value, absoluteExpireTime, keepTtl);
                return;
            case CachingType.Memory:
                await _memCaching.SetAsync(key, value, absoluteExpireTime, keepTtl);
                return;
            case CachingType.Redis:
                await _redisCaching.SetAsync(key, value, absoluteExpireTime, keepTtl);
                return;
        }
        throw new Exception("The caching type is invalid. Please re-check!!!");
    }

    public async Task RemoveAsync(string key, CachingType type = CachingType.Couple, CancellationToken cancellationToken = default)
    {
        switch (type)
        {
            case CachingType.Couple:
                await _memCaching.DeleteAsync(key);
                await _redisCaching.DeleteAsync(key);
                return;
            case CachingType.Memory:
                await _memCaching.DeleteAsync(key);
                return;
            case CachingType.Redis:
                await _redisCaching.DeleteAsync(key);
                return;
        }
        throw new Exception("The caching type is invalid. Please re-check!!!");
    }
}