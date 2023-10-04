using OnlineShop.Application.Dto.Auth;
using OnlineShop.Application.Infrastructure;
using OnlineShop.Domain.Entities;
using SharedKernel.Application.Models.Requests;
using SharedKernel.Application.Responses;

namespace OnlineShop.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    public IUnitOfWork UnitOfWork { get; }
    
    public async Task<TokenUser> GetTokenUserByIdentityAsync(string username, string password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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