using Action = OnlineShop.Domain.Entities.Action;

namespace OnlineShop.Application.Infrastructure;

public interface ICpanelReadOnlyRepository : IBaseReadOnlyRepository<BaseEntity, IApplicationDbContext>
{
    Task<IEnumerable<ApplicationUser>> GetUserByRoleIdAsync(object roleId, CancellationToken cancellationToken = default);

    Task<IEnumerable<RoleDto>> GetRolesAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Action>> GetActionsByExponentsAsync(List<ActionExponent> exponents, CancellationToken cancellationToken = default);
    
    Task<IPagedList<UserDto>> GetUserPagingAsync(PagingRequest request, CancellationToken cancellationToken = default);

    Task<IEnumerable<RecordDashboardDto>> GetRecordDashboardAsync(CancellationToken cancellationToken = default);
}