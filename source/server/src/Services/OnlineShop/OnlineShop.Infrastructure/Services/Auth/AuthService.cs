using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineShop.Infrastructure.Services;

public class AuthService : IAuthService
{

    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;
    private readonly ISequenceCaching _sequenceCaching;
    private readonly ICurrentUser _currentUser;

    public AuthService(
        IAuthRepository authRepository,
        IConfiguration configuration,
        ISequenceCaching sequenceCaching,
        ICurrentUser currentUser
        )
    {
        _authRepository = authRepository;
        _configuration = configuration;
        _sequenceCaching = sequenceCaching;
        _currentUser = currentUser;
    }
    
    public bool CheckPermission(ActionExponent exponent)
    {
        return CheckPermission(new ActionExponent[] { exponent });
    }

    public bool CheckPermission(ActionExponent[] exponents)
    {
        var length = exponents.Length;
        for (int i = 0; i < length; i++)
        {
            var action = AuthUtility.FromExponentToPermission((int)exponents[i]);
            if (!AuthUtility.ComparePermissionAsString(_currentUser.Context.Permission, action))
            {
                return false;
            }
        }
        return true;
    }

    public async Task<string> GenerateAccessTokenAsync(TokenUser tokenUser, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>();
        var secretKey = Encoding.UTF8.GetBytes(DefaultJwtConfig.Key);
        var symmetricSecurityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(secretKey);
        var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(symmetricSecurityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

        // Add claims
        claims.Add(new Claim(ClaimConstant.USER_ID, tokenUser.Id.ToString()));
        claims.Add(new Claim(ClaimConstant.USERNAME, tokenUser.Username));
        claims.Add(new Claim(ClaimConstant.ROLES, string.Join(",", tokenUser.RoleNames)));
        claims.Add(new Claim(ClaimConstant.PERMISSION, tokenUser.Permission));
        claims.Add(new Claim(ClaimConstant.CREATE_AT, tokenUser.CreatedDate.ToString()));
        claims.Add(new Claim(ClaimConstant.AUTHOR, "Đỗ Chí Hùng"));
        claims.Add(new Claim(ClaimConstant.ORGANIZATION, "Online Shop Microservices"));
        claims.Add(new Claim(ClaimConstant.AUTHORS_MESSAGE, "Contact for work: 0976580418; Facebook: https://facebook.com/dohungiy"));

        var securityToken = new JwtSecurityToken(
            issuer: DefaultJwtConfig.Issuer,
            audience: DefaultJwtConfig.Audience,
            claims: claims,
            expires: DateHelper.Now.AddSeconds(DefaultJwtConfig.ExpiredSecond),
            signingCredentials: credentials
        );

        var accessToken = tokenHandler.WriteToken(securityToken);
        
        /**
         * Save token vào redis cache
         * Nếu chỉ cho phép online trên 1 thiết bị: revoke (thu hồi) token cũ, save token mới
         * Nếu chỉ cho phép online trên nhiều thiết bị: update token
         */

        var key = BaseCacheKeys.GetAccessTokenKey(tokenUser.Id);
        var oldTokens = await _sequenceCaching.GetStringAsync(key, cancellationToken: cancellationToken);

        if (CoreSettings.IsSingleDevice)
        {
            if (!string.IsNullOrEmpty(oldTokens))
            {
                var tokens = oldTokens.Split(";");
                await _authRepository.RemoveRefreshTokenAsync(cancellationToken);
                await Task.WhenAll(tokens.Select(token => RevokeAccessTokenAsync(token, cancellationToken)).Concat(new Task[] { _sequenceCaching.RemoveAsync(key, cancellationToken: cancellationToken) }));
            }
            await _sequenceCaching.SetAsync(key, accessToken, TimeSpan.FromSeconds(DefaultJwtConfig.ExpiredSecond), cancellationToken: cancellationToken);
        }
        else
        {
            var tokenValues = string.IsNullOrEmpty(oldTokens) ? accessToken : $"{oldTokens};{accessToken}";
            await _sequenceCaching.SetAsync(key, tokenValues, TimeSpan.FromSeconds(DefaultJwtConfig.ExpiredSecond), cancellationToken: cancellationToken);
        }
        
        return accessToken;
    }

    public string GenerateRefreshToken()
    {
        return Utility.RandomString(128);
    }

    public async Task<bool> CheckRefreshTokenAsync(string value, Guid userId, CancellationToken cancellationToken)
    {
        return await _authRepository.CheckRefreshTokenAsync(value, userId, cancellationToken);
    }

    public Task<bool> SignOutAllDeviceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task RevokeAccessTokenAsync(string accessToken, CancellationToken cancellationToken)
    {
        await _sequenceCaching.SetAsync(
            BaseCacheKeys.GetRevokeAccessTokenKey(accessToken), 
            DateHelper.Now, 
            TimeSpan.FromSeconds(DefaultJwtConfig.ExpiredSecond), 
            cancellationToken: cancellationToken);
    }
}