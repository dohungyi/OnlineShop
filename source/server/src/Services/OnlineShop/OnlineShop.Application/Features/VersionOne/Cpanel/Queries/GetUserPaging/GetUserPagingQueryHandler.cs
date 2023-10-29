namespace OnlineShop.Application.Features.VersionOne;

public class GetUserPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetUserPagingQuery, ApiResult>
{
    private readonly ICpanelReadOnlyRepository _cpanelReadOnlyRepository;
    
    public GetUserPagingQueryHandler(
        IAuthService authService, 
        IMapper mapper,
        ICpanelReadOnlyRepository cpanelReadOnlyRepository) : base(authService, mapper)
    {
        _cpanelReadOnlyRepository = cpanelReadOnlyRepository;
    }

    public async Task<ApiResult> Handle(GetUserPagingQuery request, CancellationToken cancellationToken)
    {
        
        return new ApiSimpleResult()
        {
            Data = await _cpanelReadOnlyRepository.GetUserPagingAsync(request.Request, cancellationToken)
        };
    }
}