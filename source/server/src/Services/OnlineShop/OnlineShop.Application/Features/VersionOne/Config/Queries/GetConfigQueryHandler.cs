namespace OnlineShop.Application.Features.VersionOne.Config.Queries;

public class GetConfigQueryHandler : BaseQueryHandler, IRequestHandler<GetConfigQuery, ApiResult>
{
    private readonly IConfigReadOnlyRepository _readRepository;
    private readonly IConfigWriteOnlyRepository _writeRepository;
    
    public GetConfigQueryHandler(
        IAuthService authService, 
        IMapper mapper,
        IConfigReadOnlyRepository readRepository,
        IConfigWriteOnlyRepository writeRepository) : base(authService, mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task<ApiResult> Handle(GetConfigQuery request, CancellationToken cancellationToken)
    {
        var result = await _readRepository.GetConfigAsync(cancellationToken) ?? await _writeRepository.CreateOrUpdateAsync(null, cancellationToken);
        return new ApiSimpleResult()
        {
            Data = _mapper.Map<UserConfigDto>(result)
        };
    }
}