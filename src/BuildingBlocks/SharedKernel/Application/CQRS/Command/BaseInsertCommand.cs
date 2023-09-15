using MediatR;

namespace SharedKernel.Application.CQRS.Command;

public class BaseInsertCommand<TResponse> : BaseCommand<TResponse>
{
}

public class BaseInsertCommand : BaseInsertCommand<Unit>
{
}