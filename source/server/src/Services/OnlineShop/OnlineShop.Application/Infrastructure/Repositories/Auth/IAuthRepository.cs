namespace OnlineShop.Application.Infrastructure;

public interface IAuthRepository
{
    IUnitOfWork UnitOfWork { get; }

    Task<TokenUser> GetTokenUserByIdentityAsync(string username, string password, CancellationToken cancellationToken = default);

    Task<TokenUser> GetTokenUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task SignOutAsync(CancellationToken cancellationToken = default);

    Task<bool> CheckRefreshTokenAsync(string value, Guid userId, CancellationToken cancellationToken = default);

    Task CreateOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    Task RemoveRefreshTokenAsync(CancellationToken cancellationToken = default);

    Task SetRoleForUserAsync(Guid userId, List<Guid> roleIds, CancellationToken cancellationToken = default);

    Task<bool> VerifySecretKeyAsync(string secretKey, CancellationToken cancellationToken = default);

    Task<IPagedList<SignInHistoryDto>> GetSignInHistoryPaging(PagingRequest request, CancellationToken cancellationToken = default);
}