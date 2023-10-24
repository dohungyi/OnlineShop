namespace OnlineShop.Application.Features.VersionOne;

public class RefreshTokenCommand : BaseAllowAnonymousCommand<ApiResult>
{
    public Guid UserId { get; init; }
    public string RefreshToken { get; init; }

    public RefreshTokenCommand(Guid userId, string refreshToken)
    {
        UserId = userId;
        RefreshToken = refreshToken;
    }
}