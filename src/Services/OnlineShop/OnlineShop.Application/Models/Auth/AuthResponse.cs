using SharedKernel.Application.Responses;

namespace OnlineShop.Application.Models.Auth;

public class AuthResponse : ApiSuccessResult
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}