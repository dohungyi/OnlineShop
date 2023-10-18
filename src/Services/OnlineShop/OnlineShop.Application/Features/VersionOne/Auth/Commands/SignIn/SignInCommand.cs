namespace OnlineShop.Application.Features.VersionOne;

public class SignInCommand : BaseAllowAnonymousCommand<ApiResult>
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public SignInCommand(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}