using Microsoft.EntityFrameworkCore;
using SharedKernel.Application;
using SharedKernel.Domain;

namespace SharedKernel.Persistence;

public interface IAppDbContext : IUnitOfWork
{
    Task PublishEvents(IEventBus eventBus, CancellationToken cancellationToken);
}