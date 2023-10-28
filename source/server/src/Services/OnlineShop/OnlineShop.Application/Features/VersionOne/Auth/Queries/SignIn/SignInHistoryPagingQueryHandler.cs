namespace OnlineShop.Application.Features.VersionOne;

public class SignInHistoryPagingQueryHandler : BaseQueryHandler, IRequestHandler<SignInHistoryPagingQuery, IPagedList<SignInHistoryDto>>
{
    private readonly IAuthRepository _authRepository;
    public SignInHistoryPagingQueryHandler(
        IAuthService authService, 
        IMapper mapper,
        IAuthRepository authRepository
        ) : base(authService, mapper)
    {
        _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
    }

    public async Task<IPagedList<SignInHistoryDto>> Handle(SignInHistoryPagingQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}