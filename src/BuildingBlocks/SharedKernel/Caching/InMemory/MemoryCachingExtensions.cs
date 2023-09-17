using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Caching;

public static class MemoryCachingExtensions
{
    public static IServiceCollection AddInMemoryCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IMemoryCaching>(s => 
            new MemoryCaching(s.GetRequiredService<IMemoryCache>()));
        
        return services;
    }
}