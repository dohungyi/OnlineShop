
using Google.Apis.Auth;

namespace OnlineShop.Application.Features.VersionOne;


public class SignInByGoogleCommandHandler : BaseCommandHandler, IRequestHandler<SignInByGoogleCommand, ApiResult>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IAuthRepository _authRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    public SignInByGoogleCommandHandler(
        IEventDispatcher eventDispatcher, 
        IAuthService authService,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IAuthRepository authRepository, 
        IStringLocalizer<Resources> localizer
        ) : base(eventDispatcher, authService)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _authRepository = authRepository;
        _localizer = localizer;
    }

    public async Task<ApiResult> Handle(SignInByGoogleCommand request, CancellationToken cancellationToken)
    {
        var payload = await ValidateAndParseGoogleIdTokenAsync(request.IdToken);
        
        var user = await _userReadOnlyRepository.FindByEmailAsync(payload.Email, cancellationToken);
        TokenUser tokenUser = null;
        
        if (user is null)
        {
            user = GetUserFromPayload(payload);
            user = await _userWriteOnlyRepository.CreateUserAsync(user, cancellationToken);
        }
        
        tokenUser = await _authRepository.GetTokenUserByIdAsync(user.Id, cancellationToken);
        
        var authResponse = new AuthResponse()
        {
            AccessToken = await _authService.GenerateAccessTokenAsync(tokenUser, cancellationToken),
            RefreshToken = _authService.GenerateRefreshToken(),
        };
        
        // Save refresh token
        var refreshToken = new RefreshToken
        {
            RefreshTokenValue = authResponse.RefreshToken,
            CurrentAccessToken = authResponse.AccessToken,
            UserId = tokenUser.Id,
            ExpirationDate = DateHelper.Now.AddSeconds(AuthConstant.REFRESH_TOKEN_TIME),
            CreatedBy = tokenUser.Username,
        };
        
        await _authRepository.CreateOrUpdateRefreshTokenAsync(refreshToken, cancellationToken);
        await _authRepository.UnitOfWork.CommitAsync(false, cancellationToken);
        
        return new ApiSuccessResult<AuthResponse>(authResponse);
    }

    private async Task<GoogleJsonWebSignature.Payload> ValidateAndParseGoogleIdTokenAsync(string idToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>() { DefaultGoogleConfig.ClientId }
        };
        
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

        if (payload is null)
        {
            throw new BadRequestException(_localizer["auth_id_token_is_invalid"].Value);
        }

        return payload;
    }

    private ApplicationUser GetUserFromPayload(GoogleJsonWebSignature.Payload payload)
    {
        return new ApplicationUser()
        {
            Username = payload.Email,
            PasswordHash = payload.Email.ToMD5(),
            Salt = Utility.RandomString(6),
            Email = payload.Email,
            ConfirmedEmail = payload.EmailVerified,
            FirstName = payload.FamilyName,
            LastName = payload.GivenName,
            DateOfBirth = new DateTime(1990, 01, 01).ToUniversalTime(),
        };
    } 
}