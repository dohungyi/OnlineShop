using OnlineShop.Application.Dtos;
using OnlineShop.Application.Infrastructure;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Persistence;
using SharedKernel.Auth;
using SharedKernel.Caching;
using SharedKernel.Infrastructures.Repositories;
using Action = OnlineShop.Domain.Entities.Action;

namespace OnlineShop.Infrastructure.Repositories;

public class CpanelReadOnlyRepository : BaseReadOnlyRepository<BaseEntity, ApplicationDbContext>, ICpanelReadOnlyRepository
{
    public CpanelReadOnlyRepository(
        ApplicationDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider
        ) : base(dbContext, currentUser, sequenceCaching, provider)
    {
    }

    public async Task<IEnumerable<ApplicationUser>> GetUserByRoleIdAsync(object roleId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RoleDto>> GetRoleAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Action>> GetActionsByExponentsAsync(List<ActionExponent> exponents, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IPagedList<UserDto>> GetUserPagingAsync(PagingRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RecordDashboardDto>> GetRecordDashboardAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}