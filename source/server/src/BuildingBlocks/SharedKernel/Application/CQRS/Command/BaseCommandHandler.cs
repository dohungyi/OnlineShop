using SharedKernel.Domain;

namespace SharedKernel.Application;

public abstract class BaseCommandHandler
{
    protected readonly IEventDispatcher _eventDispatcher;
    protected readonly IAuthService _authService;

    public BaseCommandHandler(IEventDispatcher eventDispatcher, IAuthService authService)
    {
        _eventDispatcher = eventDispatcher;
        _authService = authService;
    }
}