using System.Collections;
using System.Reflection;
using Caching.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Caching.InMemory;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    
    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }
    
    public async Task<bool> ExistsAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        return await Task.FromResult(_cache.TryGetValue(key, out _));
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

        await SetStringAsync(key, JsonConvert.SerializeObject(value), expiry, keepTtl);

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

        if (expiry.HasValue)
        {
            _cache.Set(key, value);
        }
        else
        {
            _cache.Set(key, value, expiry.Value);
        }

        return await ExistsAsync(key);
    }

    public async Task<bool> DeleteAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        _cache.Remove(key);

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

    public async Task<T> GetAsync<T>(string key) where T : class
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

        if (_cache.Get(key) is not string result)
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
        var typeOfCache = _cache.GetType();
        var fieldEntries = typeOfCache?.GetField("_entries", flags);
        var entriesValue = fieldEntries?.GetValue(_cache);
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