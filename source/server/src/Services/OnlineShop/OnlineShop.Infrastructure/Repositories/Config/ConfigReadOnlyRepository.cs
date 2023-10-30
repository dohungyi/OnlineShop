namespace OnlineShop.Infrastructure.Repositories.Config;

public class ConfigReadOnlyRepository : BaseReadOnlyRepository<ApplicationUserConfig, ApplicationDbContext>, IConfigReadOnlyRepository
{
    public ConfigReadOnlyRepository(
        ApplicationDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider) : base(dbContext, currentUser, sequenceCaching, provider)
    {
    }

    public async Task<ApplicationUserConfig> GetConfigAsync(CancellationToken cancellationToken)
    {
        var cacheKey = BaseCacheKeys.GetConfigKey(_currentUser.Context.UserId);
        var cacheData = await _sequenceCaching.GetAsync<ApplicationUserConfig>(cacheKey, cancellationToken: cancellationToken);
        if (cacheData is not null)
        {
            return cacheData;
        }
        
        var result = await FindByCondition(
            expression: x => x.UserId.Equals(_currentUser.Context.UserId) && !x.IsDeleted,
            trackChanges: true
            ).FirstOrDefaultAsync(cancellationToken);

        return result;

    }
}