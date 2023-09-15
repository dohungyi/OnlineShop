using MediatR;

namespace SharedKernel.Application.CQRS.Command;

public class BaseUpdateCommand<TResponse> : BaseCommand<TResponse>
{
}

public class BaseUpdateCommand : BaseUpdateCommand<Unit>
{
}