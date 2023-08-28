using Caching.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Caching.InMemory;

public static class MemoryCacheExtensions
{
    public static IServiceCollection ConfigureInMemoryCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheService>(s => 
            new MemoryCacheService(s.GetRequiredService<IMemoryCache>()));
        
        return services;
    }
}