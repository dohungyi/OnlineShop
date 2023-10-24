using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SharedKernel.Application.Consts;

namespace SharedKernel.Auth;

public class CurrentUser : ICurrentUser
{
    #region [PROPERTIES]
    
    private readonly IHttpContextAccessor _accessor;
    public string Id => Guid.NewGuid().ToString();
    private ExecutionContext _context { get; set; }
    public ExecutionContext Context
    {
        get
        {
            if (_context == null)
            {
                _context = GetContext(GetAccessToken());
            }
            return _context;
        }
        set { _context = value; }
    }
    
    #endregion

    #region [CONSTRUCTORS]
    
    public CurrentUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    #endregion

    #region [PRIVATES]

    private string GetAccessToken()
    {
        var bearerToken = _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString();
        if (string.IsNullOrEmpty(bearerToken) || bearerToken.Equals("Bearer"))
        {
            return "";
        }
        
        return bearerToken.Substring(7);
    }

    private ExecutionContext GetContext(string accessToken)
    {
        var httpContext = _accessor.HttpContext;
        
        if (string.IsNullOrEmpty(accessToken))
        {
            return new ExecutionContext { HttpContext = httpContext };
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var claims = jwtSecurityToken.Claims;

        if (claims is null)
        {
            return new ExecutionContext { HttpContext = httpContext };
        }
        
        return new ExecutionContext
        {
            AccessToken = accessToken,
            UserId = claims.First(c => c.Type == ClaimConstant.USER_ID).Value,
            Username = claims.First(c => c.Type == ClaimConstant.USERNAME).Value,
            Permission = claims.First(c => c.Type == ClaimConstant.PERMISSION).Value,
            Roles = claims.First(c => c.Type == ClaimConstant.ROLES).Value,
            HttpContext = httpContext
        };
    }

    #endregion
    
}