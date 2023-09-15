using AutoMapper;

namespace SharedKernel.Application;

public abstract class BaseQueryHandler
{
    protected readonly IAuthService _authService;
    protected readonly IMapper _mapper;

    public BaseQueryHandler(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }
}