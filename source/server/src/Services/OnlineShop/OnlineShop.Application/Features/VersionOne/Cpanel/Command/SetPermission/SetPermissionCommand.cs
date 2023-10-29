using SharedKernel.Libraries;

namespace OnlineShop.Application.Features.VersionOne;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.SupperAdmin })]
public class SetPermissionCommand
{
    public string OwnerId { get; init; }
    public string Exponent { get; init; }

    public SetPermissionCommand(string ownerId, string exponent)
    {
        OwnerId = ownerId;
        Exponent = exponent;
    }
}