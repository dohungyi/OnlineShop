using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        var users = await _dbContext.ApplicationUsers
            .Join(_dbContext.UserRoles,
                user => user.Id,
                userRole => userRole.UserId,
                (user, userRole) => new { User = user, UserRole = userRole })
            .Where(x => x.UserRole.RoleId == (Guid)roleId && !x.User.IsDeleted && !x.UserRole.IsDeleted)
            .Select(x => x.User)
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task<IEnumerable<RoleDto>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Action>> GetActionsByExponentsAsync(List<ActionExponent> exponents, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IPagedList<UserDto>> GetUserPagingAsync(PagingRequest request, CancellationToken cancellationToken = default)
    {
        var brr = _provider.GetRequiredService<IBaseReadOnlyRepository<ApplicationUser, IApplicationDbContext>>();
        
        var users  = await brr.GetPagingAsync(request, cancellationToken);

        var mapper = _provider.GetRequiredService<IMapper>();
        
        var usersDto = mapper.Map<IPagedList<UserDto>>(users);
        
        return usersDto;
    }

    public async Task<IEnumerable<RecordDashboardDto>> GetRecordDashboardAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}