using SharedKernel.Application;

namespace SharedKernel.Application;

public interface IAuthService
{
    bool CheckPermission(ActionExponent exponent);
    
    bool CheckPermission(ActionExponent[] exponents);
    
    Task<string> GenerateAccessTokenAsync(TokenUser token, CancellationToken cancellationToken);
    
    string GenerateRefreshToken();
    
    Task RevokeAccessTokenAsync(string accessToken, CancellationToken cancellationToken);
    
    Task<bool> CheckRefreshTokenAsync(string value, long ownerId, CancellationToken cancellationToken);
}