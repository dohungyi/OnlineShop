using System.Collections;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace SharedKernel.Caching;

public class MemoryCaching : IMemoryCaching
{
    private readonly IMemoryCache _caching;
    
    public MemoryCaching(IMemoryCache caching)
    {
        _caching = caching;
    }
    
    public TimeSpan DefaultAbsoluteExpireTime => TimeSpan.FromHours(2);
    
    public async Task<bool> ExistsAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        return await Task.FromResult(_caching.TryGetValue(key, out _));
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null, bool keepTtl = false)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        var absoluteExpireTime = expiry ?? DefaultAbsoluteExpireTime;
        
        await SetStringAsync(key, JsonConvert.SerializeObject(value), absoluteExpireTime, keepTtl);

        return await ExistsAsync(key);
    }

    public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null, bool keepTtl = false)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (!expiry.HasValue)
        {
            _caching.Set(key, value, DefaultAbsoluteExpireTime);
        }
        else
        {
            _caching.Set(key, value, expiry.Value);
        }

        return await ExistsAsync(key);
    }

    public async Task<bool> DeleteAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        _caching.Remove(key);

        return !(await ExistsAsync(key));
    }

    public async Task<bool> DeleteByPatternAsync(string pattern)
    {
        var allKey = GetAllCacheKeys();
        var removeKeys = allKey.Where(x => x.StartsWith(pattern)).ToList();
        if (removeKeys.Any())
        {
            foreach (var key in removeKeys)
            {
                await DeleteAsync(key);
            }
        }

        return true;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        var resultRaw = await GetStringAsync(key);
        if (string.IsNullOrWhiteSpace(resultRaw))
        {
            return default!;
        }
        
        return JsonConvert.DeserializeObject<T>(resultRaw);
    }

    public async Task<string> GetStringAsync(string key)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (_caching.Get(key) is not string result)
        {
            return default!;
        }

        return await Task.FromResult(result);
    }

    public async Task<bool> ReplaceAsync(string key, object value)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (await ExistsAsync(key) && !await DeleteAsync(key))
        {
            return false;
        }

        return await SetAsync(key, value);
    }

    #region [PRIVATE METHODS]

    private List<string> GetAllCacheKeys()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        // var entries = _cache.GetType()?.GetField("_entries", flags)?.GetValue(_cache);
        // var cacheItems = entries as IDictionary;
        var typeOfCache = _caching.GetType();
        var fieldEntries = typeOfCache?.GetField("_entries", flags);
        var entriesValue = fieldEntries?.GetValue(_caching);
        var cacheItems = entriesValue as IDictionary;

        if (cacheItems is null || cacheItems.Count <= 0)
        {
            return default!;
        }
        
        var keys = new List<string>();
        foreach (DictionaryEntry cacheItem in cacheItems)
        {
            keys.Add(cacheItem.Key.ToString());
        }
        return keys;
    }
    

    #endregion
}