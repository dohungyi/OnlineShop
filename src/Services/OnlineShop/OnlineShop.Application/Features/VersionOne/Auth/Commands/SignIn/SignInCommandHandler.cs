using System.Net;
using MediatR;
using Microsoft.Extensions.Localization;
using OnlineShop.Application.Constants;
using OnlineShop.Application.Infrastructure;
using OnlineShop.Application.Models.Auth;
using OnlineShop.Application.Properties;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Events.Auth;
using SharedKernel.Libraries;
using SharedKernel.Libraries.Utility;
using SharedKernel.Runtime.Exceptions;

namespace OnlineShop.Application.Features.VersionOne;

public class SignInCommandHandler : BaseCommandHandler, IRequestHandler<SignInCommand, ApiResult>
{
    private readonly IAuthRepository _authRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    public SignInCommandHandler(
        IEventBus eventBus,
        IAuthService authService,
        IAuthRepository authRepository,
        IStringLocalizer<Resources> localizer
        ) : base(eventBus, authService)
    {
        _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }

    public async Task<ApiResult> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var tokenUser =
            await _authRepository.GetTokenUserByIdentityAsync(request.UserName, request.Password, cancellationToken);

        if (tokenUser is null)
        {
            throw new BadRequestException(_localizer["auth_sign_in_info_incorrect"].Value);
            // return new ApiErrorResult
            // {
            //     Error = new Error(HttpStatusCode.BadRequest, _localizer["auth_sign_in_info_incorrect"].Value)
            // };
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
            CreatedDate = DateHelper.Now
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

        return authResponse;
    }
}