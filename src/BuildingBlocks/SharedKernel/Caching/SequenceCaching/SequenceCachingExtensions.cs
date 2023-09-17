using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Caching;

public static class SequenceCachingExtensions
{
    public static IServiceCollection AddSequenceCaching(this IServiceCollection services)
    {
        services.AddSingleton<ISequenceCaching, SequenceCaching>();
        return services;
    }
}