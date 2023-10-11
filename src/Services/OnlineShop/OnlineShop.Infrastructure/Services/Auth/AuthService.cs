namespace OnlineShop.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    public bool CheckPermission(ActionExponent exponent)
    {
        throw new NotImplementedException();
    }

    public bool CheckPermission(ActionExponent[] exponents)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GenerateAccessTokenAsync(TokenUser token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public async Task RevokeAccessTokenAsync(string accessToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CheckRefreshTokenAsync(string value, long ownerId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}