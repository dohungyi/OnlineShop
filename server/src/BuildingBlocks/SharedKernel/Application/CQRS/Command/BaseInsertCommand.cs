using MediatR;

namespace SharedKernel.Application;

public class BaseInsertCommand<TResponse> : BaseCommand<TResponse>
{
}

public class BaseInsertCommand : BaseInsertCommand<Unit>
{
}