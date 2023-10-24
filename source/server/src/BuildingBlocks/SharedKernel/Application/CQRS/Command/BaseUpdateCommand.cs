using MediatR;

namespace SharedKernel.Application;

public class BaseUpdateCommand<TResponse> : BaseCommand<TResponse>
{
}

public class BaseUpdateCommand : BaseUpdateCommand<Unit>
{
}