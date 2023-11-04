namespace OnlineShop.Application.Features.VersionOne;

public class RefreshTokenCommandHandler : BaseCommandHandler, IRequestHandler<RefreshTokenCommand, ApiResult>
{
    private readonly IAuthRepository _authRepository;
    public RefreshTokenCommandHandler(
        IEventDispatcher eventDispatcher, 
        IAuthService authService,
        IAuthRepository authRepository) 
        : base(eventDispatcher, authService)
    {
        _authRepository = authRepository;
    }

    public async Task<ApiResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenUser = await _authRepository.GetTokenUserByIdAsync(request.UserId, cancellationToken);
        if (tokenUser is null)
        {
            throw new BadRequestException("Currently, this account does not exist");
        }
        
        var isValid = await _authService.CheckRefreshTokenAsync(request.RefreshToken, request.UserId, cancellationToken);
        if (!isValid)
        {
            throw new BadRequestException("The refresh-token is invalid or expired!");
        }
        
        var currentAccessToken = await _authService.GenerateAccessTokenAsync(tokenUser, cancellationToken);
        var refreshToken = new RefreshToken
        {
            RefreshTokenValue = request.RefreshToken,
            CurrentAccessToken= currentAccessToken,
            UserId = request.UserId,
            ExpirationDate = DateHelper.Now.AddSeconds(AuthConstant.REFRESH_TOKEN_TIME)
        };
        await _authRepository.CreateOrUpdateRefreshTokenAsync(refreshToken, cancellationToken);
        await _authRepository.UnitOfWork.CommitAsync(false, cancellationToken);

        var authResponse = new AuthResponse
        {
            AccessToken = currentAccessToken,
            RefreshToken = request.RefreshToken
        };
        
        return new ApiSuccessResult<AuthResponse>(authResponse);
    }
}