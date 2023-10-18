namespace OnlineShop.Application.Features.VersionOne;

public class SignOutAllDeviceCommand : BaseAllowAnonymousCommand<ApiResult>
{
    public Guid UserId { get; init; }
    
    public SignOutAllDeviceCommand(Guid userId)
    {
        UserId = userId;
    }
}