using AutoMapper.QueryableExtensions;

namespace OnlineShop.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IServiceProvider _provider;

    public AuthRepository(
        IApplicationDbContext context, 
        ICurrentUser currentUser, 
        IServiceProvider provider)
    {
        _context = context;
        _currentUser = currentUser;
        _provider = provider;
    }

    public IUnitOfWork UnitOfWork => _context;
    
    public async Task<TokenUser> GetTokenUserByIdentityAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        return await GetTokenUserByIdentityOrUserIdAsync(username, password, null, cancellationToken);
    }

    public async Task<TokenUser> GetTokenUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetTokenUserByIdentityOrUserIdAsync(null, null, userId, cancellationToken);
    }

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        await RemoveRefreshTokenAsync(cancellationToken);
    }

    public async Task<bool> CheckRefreshTokenAsync(string value, Guid userId, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => 
                rt.RefreshTokenValue == value 
                && rt.UserId == userId 
                && rt.ExpirationDate >= DateHelper.Now, 
                cancellationToken);
        
        return refreshToken is not null;
    }

    public async Task CreateOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        var existingRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == refreshToken.UserId, cancellationToken);

        if (existingRefreshToken is null)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }
        else
        {
            existingRefreshToken.RefreshTokenValue = refreshToken.RefreshTokenValue;
            existingRefreshToken.CurrentAccessToken = refreshToken.CurrentAccessToken;
            existingRefreshToken.ExpirationDate = refreshToken.ExpirationDate;
            
            _context.RefreshTokens.Update(existingRefreshToken);
        }
    }

    public async Task RemoveRefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.CurrentAccessToken == _currentUser.Context.AccessToken, cancellationToken);

        if (refreshToken is null)
        {
            return;
        }
        
        _context.RefreshTokens.Remove(refreshToken);
    }

    public async Task SetRoleForUserAsync(Guid userId, List<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        _context.UserRoles.AddRange(roleIds.Select(r => new UserRole { UserId = userId, RoleId = userId }));
    }

    public async Task<bool> VerifySecretKeyAsync(string secretKey, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IPagedList<SignInHistoryDto>> GetSignInHistoryPaging(PagingRequest request, CancellationToken cancellationToken = default)
    {
        var mapper = _provider.GetRequiredService<IMapper>();

        var signInHistoryPaging = await _context.SignInHistories
            .Where(s => s.Username == _currentUser.Context.Username &&
                        s.UserId.ToString() == _currentUser.Context.UserId)
            .ProjectTo<SignInHistoryDto>(mapper.ConfigurationProvider)
            .ToPagedListAsync(request.Page, request.Size, request.IndexForm, cancellationToken);

        return signInHistoryPaging;
    }

    #region [Private]

    private async Task<TokenUser> GetTokenUserByIdentityOrUserIdAsync(string? username, string? password, Guid? userId, CancellationToken cancellationToken)
    {
        var user = await _context.ApplicationUsers
            .Where(u => 
                (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && u.Username == username && u.PasswordHash == password.ToMD5()) 
                        || u.Id == userId)
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .ThenInclude(u => u.RoleActions)
            .ThenInclude(u => u.Action)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return null!;
        }
        
        var tokenUser = new TokenUser()
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            Salt = user.Salt,
            PhoneNumber = user.PhoneNumber,
            ConfirmedPhone = user.ConfirmedPhone,
            Email = user.Email,
            ConfirmedEmail = user.ConfirmedEmail,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
        };

        var roleActions = user.UserRoles?.SelectMany(u => u.Role.RoleActions);
        
        if (roleActions is null)
        {
            return tokenUser;
        }
        
        var sa = roleActions.FirstOrDefault(x => x.Action.Code.Equals(RoleConstant.SupperAdmin));
        var admin = roleActions.FirstOrDefault(x => x.Action.Code.Equals(RoleConstant.Admin));
        
        if (sa is not null)
        {
            tokenUser.Permission = AuthUtility.CalculateToTalPermission(Enumerable.Range(0, sa.Action.Exponent + 1));
        }
        else if (admin is not null)
        {
            tokenUser.Permission = AuthUtility.CalculateToTalPermission(Enumerable.Range(0, admin.Action.Exponent + 1));
        }
        else
        {
            tokenUser.Permission = AuthUtility.CalculateToTalPermission(roleActions.Select(x => x.Action.Exponent));
        }

        tokenUser.RoleNames = roleActions.DistinctBy(x => x.Role.Name).Select(x => x.Role.Name).ToList();
        
        return tokenUser;
    }
    

    #endregion
}