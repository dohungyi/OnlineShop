
using Microsoft.Extensions.Localization;
using OnlineShop.Application.Constants;
using OnlineShop.Application.Dto.Auth;
using OnlineShop.Application.Infrastructure;
using OnlineShop.Application.Properties;
using OnlineShop.Application.Repositories;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Repositories;
using SharedKernel.Core;
using SharedKernel.Libraries;
using SharedKernel.Runtime.Exceptions;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace OnlineShop.Application.Features.VersionOne;

public class SignInByGoogleCommandHandler : BaseCommandHandler, IRequestHandler<SignInByGoogleCommand, ApiResult>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IAuthRepository _authRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    public SignInByGoogleCommandHandler(
        IEventBus eventBus, 
        IAuthService authService,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IAuthRepository authRepository, 
        IStringLocalizer<Resources> localizer
        ) : base(eventBus, authService)
    {
        _userReadOnlyRepository = userReadOnlyRepository ?? throw new ArgumentNullException(nameof(userReadOnlyRepository));
        _userWriteOnlyRepository = userWriteOnlyRepository ?? throw new ArgumentNullException(nameof(userWriteOnlyRepository));
        _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }

    public async Task<ApiResult> Handle(SignInByGoogleCommand request, CancellationToken cancellationToken)
    {
        Payload payload = await ValidateAsync(request.IdToken, new ValidationSettings
        {
            Audience = new[] { DefaultGoogleConfig.ClientId }
        });

        var user = await _userReadOnlyRepository.FindByEmailAsync(payload.Email, cancellationToken);
        TokenUser tokenUser = null;
        
        if (user is null)
        {
            tokenUser = await _authRepository.GetTokenUserByIdAsync(user.Id, cancellationToken);
        }
        else
        {
            // Create user 
            var userNew = new ApplicationUser()
            {
                
            };

            await _userWriteOnlyRepository.CreateUserAsync(userNew, cancellationToken);
            tokenUser = await _authRepository.GetTokenUserByIdAsync(userNew.Id, cancellationToken);
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
        
        // Publish event
        // var @event = new SignInEvent(_currentUser, Guid.NewGuid(), new
        // {
        //     TokenUser = tokenUser,
        //     RequestId = AuthUtility.GetCurrentRequestId(_currentUser.Context.HttpContext)
        // });
        //
        // _currentUser.Context.AccessToken = authResponse.AccessToken;
        // _currentUser.Context.UserId = tokenUser.Id.ToString(); 
        // _ = _eventBus.PublishEvent(@event, cancellationToken);
        // _ = _eventBus.PublishEvent(new SignInAuditEvent(_currentUser), cancellationToken);

        return new ApiSuccessResult<AuthResponse>(authResponse);
    }
    
}