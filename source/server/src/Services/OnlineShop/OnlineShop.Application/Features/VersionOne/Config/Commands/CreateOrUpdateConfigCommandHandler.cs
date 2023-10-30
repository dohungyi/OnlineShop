using Newtonsoft.Json;

namespace OnlineShop.Application.Features.VersionOne;

public class CreateOrUpdateConfigCommandHandler : BaseCommandHandler, IRequestHandler<CreateOrUpdateConfigCommand, ApiResult>
{
    private readonly IConfigWriteOnlyRepository _configWriteOnlyRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public CreateOrUpdateConfigCommandHandler(IEventBus eventBus, 
        IAuthService authService,
        IConfigWriteOnlyRepository configWriteOnlyRepository, 
        ICurrentUser currentUser, 
        IMapper mapper
        ) : base(eventBus, authService)
    {
        _configWriteOnlyRepository = configWriteOnlyRepository;
        _currentUser = currentUser;
        _mapper = mapper;
    }
    

    public async Task<ApiResult> Handle(CreateOrUpdateConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = new ApplicationUserConfig()
        {
            Json = JsonConvert.SerializeObject(request.ConfigValue),
            UserId = Guid.Parse(_currentUser.Context.UserId)
        };
        
        var config = await _configWriteOnlyRepository.CreateOrUpdateAsync(entity, cancellationToken);

        return new ApiSimpleResult { Data = config };
    }
}