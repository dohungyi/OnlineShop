using OnlineShop.Application.Dto.Auth;
using OnlineShop.Domain.Entities;
using SharedKernel.Application;
using SharedKernel.Application.Models.Requests;
using SharedKernel.Application.Responses;

namespace OnlineShop.Application.Infrastructure;

public interface IAuthRepository
{
    IUnitOfWork UnitOfWork { get; }

    Task<TokenUser> GetTokenUserByIdentityAsync(string username, string password, CancellationToken cancellationToken);

    Task<TokenUser> GetTokenUserByIdAsync(Guid userId, CancellationToken cancellationToken);

    Task SignOutAsync(CancellationToken cancellationToken);

    Task<bool> CheckRefreshTokenAsync(string value, Guid userId, CancellationToken cancellationToken);

    Task CreateOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);

    Task RemoveRefreshTokenAsync(CancellationToken cancellationToken);

    Task SetRoleForUserAsync(Guid userId, List<long> roleIds, CancellationToken cancellationToken);

    Task<bool> VerifySecretKeyAsync(string secretKey, CancellationToken cancellationToken);

    Task<IPagedList<SignInHistoryDto>> GetSignInHistoryPaging(PagingRequest request, CancellationToken cancellationToken);
}