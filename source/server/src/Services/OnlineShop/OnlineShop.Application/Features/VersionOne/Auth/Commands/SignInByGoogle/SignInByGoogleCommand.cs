namespace OnlineShop.Application.Features.VersionOne;

public class SignInByGoogleCommand : BaseAllowAnonymousCommand<ApiResult>
{
    public const string PROVIDER = "google";
    
    public string IdToken { get; init; }

    public SignInByGoogleCommand(string idToken)
    {
        IdToken = idToken;
    }
}