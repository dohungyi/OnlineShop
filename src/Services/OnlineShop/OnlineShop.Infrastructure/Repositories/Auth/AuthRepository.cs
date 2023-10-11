using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Dto.Auth;
using OnlineShop.Application.Infrastructure;
using OnlineShop.Application.Infrastructure.Persistence;
using OnlineShop.Domain.Entities;
using SharedKernel.Application.Models.Requests;
using SharedKernel.Application.Responses;
using SharedKernel.Auth;
using SharedKernel.Libraries;

namespace OnlineShop.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IServiceProvider _provider;

    public AuthRepository(IApplicationDbContext context, ICurrentUser currentUser, IServiceProvider provider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public IUnitOfWork UnitOfWork => _context;
    
    public async Task<TokenUser> GetTokenUserByIdentityAsync(string username, string password, CancellationToken cancellationToken)
    {
        var tokenUser = await _context.ApplicationUsers
            .Where(u => u.Username == username && u.PasswordHash == password.ToMD5())
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .ThenInclude(u => u.RoleActions)
            .ThenInclude(u => u.Action)
            .Select(u => new TokenUser()
            {
                Username = u.Username,
                PasswordHash = u.PasswordHash,
                Salt = u.Salt,
                PhoneNumber = u.PhoneNumber,
                ConfirmedPhone = u.ConfirmedPhone,
                Email = u.Email,
                ConfirmedEmail = u.ConfirmedEmail,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DateOfBirth = u.DateOfBirth,
                Gender = u.Gender,
                Permission = string.Join(",", u.UserRoles.SelectMany(ur => ur.Role.RoleActions.Select(ra => ra.Action.Name))),
                RoleNames = u.UserRoles.Select(ur => ur.Role.Name).ToList(),
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (tokenUser is null)
        {
            return null;
        }
        
        // handler ......

        return tokenUser;
    }

    public async Task<TokenUser> GetTokenUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SignOutAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CheckRefreshTokenAsync(string value, Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task CreateOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveRefreshTokenAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SetRoleForUserAsync(Guid userId, List<long> roleIds, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> VerifySecretKeyAsync(string secretKey, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IPagedList<SignInHistoryDto>> GetSignInHistoryPaging(PagingRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}