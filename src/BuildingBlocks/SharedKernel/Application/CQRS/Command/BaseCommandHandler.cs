using SharedKernel.Domain;

namespace SharedKernel.Application.CQRS.Command;

public abstract class BaseCommandHandler
{
    protected readonly IEventBus _eventBus;
    protected readonly IAuthService _authService;

    public BaseCommandHandler(IEventBus eventBus, IAuthService authService)
    {
        _eventBus = eventBus;
        _authService = authService;
    }
}