namespace OnlineShop.Application.Features.VersionOne.Cpanel.Queries.GetUserPaging;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.SupperAdmin })]
public class GetUserPagingQuery : BaseQuery<ApiResult>
{
    public PagingRequest Request { get; init; }

    public GetUserPagingQuery(PagingRequest request)
    {
        Request = request;
    }
}