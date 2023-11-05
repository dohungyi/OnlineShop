using OnlineShop.Application.Constants;
using OnlineShop.Application.Infrastructure;
using OnlineShop.Domain.Events.Auth;
using SharedKernel.Libraries;
using SharedKernel.Runtime.Exceptions;

namespace OnlineShop.Application.Features.VersionOne;

public class SignInCommandHandler : BaseCommandHandler, IRequestHandler<SignInCommand, ApiResult>
{
    private readonly IAuthRepository _authRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly ICurrentUser _currentUser;
    
    public SignInCommandHandler(
        IEventDispatcher eventDispatcher,
        IAuthService authService,
        IAuthRepository authRepository,
        ICurrentUser currentUser,
        IStringLocalizer<Resources> localizer
        ) : base(eventDispatcher, authService)
    {
        _authRepository = authRepository;
        _localizer = localizer;
        _currentUser = currentUser;
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

        // Publish events
        var @event = new SignInEvent(_currentUser, Guid.NewGuid(), new
        {
            TokenUser = tokenUser,
            RequestId = AuthUtility.GetCurrentRequestId(_currentUser.Context.HttpContext)
        });
        
        _currentUser.Context.AccessToken = authResponse.AccessToken;
        _currentUser.Context.UserId = tokenUser.Id.ToString();
        
        _ = _eventDispatcher.PublishEvent(@event, cancellationToken);
        _ = _eventDispatcher.PublishEvent(new SignInAuditEvent(_currentUser), cancellationToken);
        
        return new ApiSuccessResult<AuthResponse>(authResponse);
    }
}