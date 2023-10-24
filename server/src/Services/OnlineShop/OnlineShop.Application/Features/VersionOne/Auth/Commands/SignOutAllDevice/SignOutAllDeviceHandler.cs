namespace OnlineShop.Application.Features.VersionOne.Auth.Commands.SignOutAllDevice;

public class SignOutAllDeviceHandler :  BaseCommandHandler, IRequestHandler<SignOutAllDeviceCommand, ApiResult>
{
    public SignOutAllDeviceHandler(IEventBus eventBus, IAuthService authService) : base(eventBus, authService)
    {
    }

    public async Task<ApiResult> Handle(SignOutAllDeviceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}