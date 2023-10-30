using MongoDB.Driver.Linq;
using Newtonsoft.Json;

namespace OnlineShop.Infrastructure.Repositories.Config;

public class ConfigWriteOnlyRepository : BaseWriteOnlyRepository<ApplicationUserConfig, ApplicationDbContext>, IConfigWriteOnlyRepository
{
    public ConfigWriteOnlyRepository(
        ApplicationDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching
        ) : base(dbContext, currentUser, sequenceCaching)
    {
    }

    public async Task<ApplicationUserConfig> CreateOrUpdateAsync(ApplicationUserConfig userConfig, CancellationToken cancellationToken)
    {
        userConfig ??= new ApplicationUserConfig {
            Json = JsonConvert.SerializeObject(new ConfigValue
            {
                Language = "en-US"
            }),
            UserId = Guid.Parse(_currentUser.Context.UserId)
        };

        var uConfig = await _dbContext.ApplicationUserConfigs
            .Where(x => x.UserId.Equals(_currentUser.Context.UserId) && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (uConfig is null)
        {
            uConfig = await InsertAsync(userConfig, cancellationToken);
        }
        else
        {
            uConfig.Json = JsonConvert.SerializeObject(userConfig.Json);
            await UpdateAsync(userConfig, cancellationToken);
        }

        await _dbContext.CommitAsync(false, cancellationToken);
        
        var cacheKey = BaseCacheKeys.GetConfigKey(_currentUser.Context.UserId);
        await _sequenceCaching.SetAsync(cacheKey, uConfig, TimeSpan.FromDays(30), cancellationToken: cancellationToken);

        return uConfig;
    }
}