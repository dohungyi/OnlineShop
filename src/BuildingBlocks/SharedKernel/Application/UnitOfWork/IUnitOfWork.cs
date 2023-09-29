using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace SharedKernel.Application;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default);
}
