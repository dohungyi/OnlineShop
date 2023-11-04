using OnlineShop.Application.Constants;
using OnlineShop.Application.Infrastructure;
using SharedKernel.Libraries;
using SharedKernel.Runtime.Exceptions;

namespace OnlineShop.Application.Features.VersionOne;

public class SignInCommandHandler : BaseCommandHandler, IRequestHandler<SignInCommand, ApiResult>
{
    private readonly IAuthRepository _authRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    public SignInCommandHandler(
        IEventDispatcher eventDispatcher,
        IAuthService authService,
        IAuthRepository authRepository,
        IStringLocalizer<Resources> localizer
        ) : base(eventDispatcher, authService)
    {
        _authRepository = authRepository;
        _localizer = localizer;
    }

    public async Task<ApiResult> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var tokenUser =
            await _authRepository.GetTokenUserByIdentityAsync(request.UserName, request.Password, cancellationToken);

        if (tokenUser is null)
        {
            throw new BadRequestException(_localizer["auth_sign_in_info_incorrect"].Value);
        }
        
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
}