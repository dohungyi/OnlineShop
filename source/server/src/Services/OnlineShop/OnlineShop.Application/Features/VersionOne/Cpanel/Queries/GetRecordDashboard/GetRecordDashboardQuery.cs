namespace OnlineShop.Application.Features.VersionOne;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.SupperAdmin })]
public class GetRecordDashboardQuery : BaseQuery<ApiResult>
{
}