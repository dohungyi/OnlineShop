namespace OnlineShop.Application.Features.VersionOne;

public class SignInHistoryPagingQuery : BaseQuery<IPagedList<SignInHistoryDto>>
{
    public PagingRequest PagingRequest { get; }

    public SignInHistoryPagingQuery(PagingRequest pagingRequest)
    {
        PagingRequest = pagingRequest;
    }
}