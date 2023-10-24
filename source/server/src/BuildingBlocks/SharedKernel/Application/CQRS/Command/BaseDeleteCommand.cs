using MediatR;

namespace SharedKernel.Application;

public class BaseDeleteCommand<TResponse> : BaseCommand<TResponse>
{
}

public class BaseDeleteCommand : BaseDeleteCommand<Unit>
{
}