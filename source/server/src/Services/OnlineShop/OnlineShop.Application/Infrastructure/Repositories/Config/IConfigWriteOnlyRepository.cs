namespace OnlineShop.Application.Infrastructure;

public interface IConfigWriteOnlyRepository : IBaseWriteOnlyRepository<ApplicationUserConfig, IApplicationDbContext>
{
    Task<ApplicationUserConfig> CreateOrUpdateAsync(ApplicationUserConfig userConfig, CancellationToken cancellationToken);
}