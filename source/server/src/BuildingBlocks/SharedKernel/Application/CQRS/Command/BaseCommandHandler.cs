using SharedKernel.Domain;

namespace SharedKernel.Application;

public abstract class BaseCommandHandler
{
    protected readonly IEventDispatcher EventDispatcher;
    protected readonly IAuthService _authService;

    public BaseCommandHandler(IEventDispatcher eventDispatcher, IAuthService authService)
    {
        EventDispatcher = eventDispatcher;
        _authService = authService;
    }
}