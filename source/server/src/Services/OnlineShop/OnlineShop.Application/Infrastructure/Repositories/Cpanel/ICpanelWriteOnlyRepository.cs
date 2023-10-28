namespace OnlineShop.Application.Infrastructure;

public interface ICpanelWriteOnlyRepository : IBaseWriteOnlyRepository<BaseEntity, IApplicationDbContext>
{
    Task UpdateRoleAsync(object roleId, object actionId, bool value, CancellationToken cancellationToken = default);
}