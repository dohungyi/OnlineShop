namespace OnlineShop.Application.Features.VersionOne;

public class GetRecordDashboardQueryHandler : BaseQueryHandler, IRequestHandler<GetRecordDashboardQuery, ApiResult>
{
    private readonly ICpanelReadOnlyRepository _cpanelReadOnlyRepository;
    
    public GetRecordDashboardQueryHandler(
        IAuthService authService, 
        IMapper mapper,
        ICpanelReadOnlyRepository cpanelReadOnlyRepository
        ) : base(authService, mapper)
    {
        _cpanelReadOnlyRepository = cpanelReadOnlyRepository;
    }

    public async Task<ApiResult> Handle(GetRecordDashboardQuery request, CancellationToken cancellationToken)
    {
        var records = await _cpanelReadOnlyRepository.GetRecordDashboardAsync(cancellationToken);

        var result = new ApiSimpleResult()
        {
            Data = records
        };

        return result;
    }
}