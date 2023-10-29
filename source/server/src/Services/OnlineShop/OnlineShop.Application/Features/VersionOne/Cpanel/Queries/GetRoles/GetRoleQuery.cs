namespace OnlineShop.Application.Features.VersionOne;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.SupperAdmin })]
public class GetRolesQuery : BaseQuery<ApiResult>
{
}