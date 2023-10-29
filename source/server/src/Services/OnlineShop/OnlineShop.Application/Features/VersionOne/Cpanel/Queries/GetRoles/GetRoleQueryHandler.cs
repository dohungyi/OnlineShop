namespace OnlineShop.Application.Features.VersionOne;

public class GetRolesQueryHandler : BaseQueryHandler, IRequestHandler<GetRolesQuery, ApiResult>
{
    private readonly ICpanelReadOnlyRepository _cpanelReadOnlyRepository;

    public GetRolesQueryHandler(
        IAuthService authService,
        IMapper mapper,
        ICpanelReadOnlyRepository cpanelReadOnlyRepository
    ) : base(authService, mapper)
    {
        _cpanelReadOnlyRepository = cpanelReadOnlyRepository;
    }
    
    public async Task<ApiResult> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _cpanelReadOnlyRepository.GetRolesAsync(cancellationToken);

        var result = new ApiSimpleResult()
        {
            Data = roles
        };

        return result;
    }
}