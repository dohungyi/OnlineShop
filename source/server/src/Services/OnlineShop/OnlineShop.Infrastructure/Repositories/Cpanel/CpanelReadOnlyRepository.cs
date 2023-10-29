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
        var result = new List<RoleDto>();
        
        var roles = await _dbContext.Roles
            .Where(r => !r.IsDeleted)
            .AsNoTracking()
            .Include(r => r.RoleActions)
            .ToListAsync(cancellationToken);

        var actions = await _dbContext.Actions
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (!roles.Any())
        {
            return default!;
        }

        foreach (var role in roles)
        {
            var actionDto = actions.Select(action =>
            {
                var dto = new ActionDto
                {
                    Id = action.Id,
                    Code = action.Code,
                    Name = action.Name,
                    Description = action.Description,
                    Exponent = action.Exponent
                };
                
                if (role.RoleActions.FirstOrDefault(x => x.RoleId == role.Id && x.ActionId == action.Id) is not null)
                {
                    dto.IsContain = true;
                }
                
                return dto;
            });
            
            result.Add(new RoleDto
            {
                Id = role.Id,
                RoleCode = role.Code,
                RoleName = role.Name,
                Actions = actionDto.ToList()
            });
        }

        return result;
    }

    public async Task<IEnumerable<Action>> GetActionsByExponentsAsync(List<ActionExponent> exponents, CancellationToken cancellationToken = default)
    {
        var exs = exponents.Select(e => (int)e);
        
        var actions = await _dbContext.Actions
            .Where(x => exs.Contains(x.Exponent))
            .ToListAsync(cancellationToken);

        return actions;
    }

    public async Task<IPagedList<UserDto>> GetUserPagingAsync(PagingRequest request, CancellationToken cancellationToken = default)
    {
        var brr = _provider.GetRequiredService<IBaseReadOnlyRepository<ApplicationUser, ApplicationDbContext>>();
        
        var users  = await brr.GetPagingAsync(request, cancellationToken);

        var mapper = _provider.GetRequiredService<IMapper>();
        
        var usersDto = mapper.Map<PagedList<UserDto>>(users);
        
        return usersDto;
    }

    public async Task<IEnumerable<RecordDashboardDto>> GetRecordDashboardAsync(CancellationToken cancellationToken = default)
    {
        List<RecordDashboardDto> result = new List<RecordDashboardDto>();
        
        // User
        var users = await _dbContext.ApplicationUsers.CountAsync(cancellationToken);
        result.Add(new RecordDashboardDto()
        {
            Type = "users",
            Title = "Tổng số người dùng",
            Value = users
        });
        
        // request
        var requests = await _dbContext.RequestInformations.CountAsync(cancellationToken);
        result.Add(new RecordDashboardDto()
        {
            Type = "requests",
            Title = "Tổng số yêu cầu",
            Value = requests
        });
        
        // Orders
        // var orders = await _dbContext.ApplicationUsers.CountAsync(cancellationToken);
        
        // Tổng doanh thu

        return result;
    }
}