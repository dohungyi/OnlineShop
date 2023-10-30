namespace OnlineShop.Application.Infrastructure;

public interface IConfigReadOnlyRepository : IBaseReadOnlyRepository<ApplicationUserConfig, IApplicationDbContext>
{
    Task<ApplicationUserConfig> GetConfigAsync(CancellationToken cancellationToken);
}